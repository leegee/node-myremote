import { WebSocketServer } from 'ws';
import http from 'http';
import fs from 'fs';
import path from 'path';
import os from 'os';
import { fileURLToPath } from 'url';

import Tray from 'trayicon';
import robot from 'robotjs';
import { windowManager } from 'node-window-manager';
import dotenv from 'dotenv';
import open from 'open';

let TRAY;

const REQUIRED_ENV_VARS = [ 'VITE_WS_PORT', 'VITE_APP_TITLE', 'VITE_APP_RE' ];
const VALID_MODIFIERS = [ 'control', 'shift', 'alt', 'command' ];
const __filename = fileURLToPath( import.meta.url );
const __dirname = path.dirname( __filename );
const DIST_DIR = path.join( __dirname, '..', 'dist' );

const getLocalIP = () => {
    const interfaces = os.networkInterfaces();
    for ( const name of Object.keys( interfaces ) ) {
        for ( const net of interfaces[ name ] ) {
            // 'IPv4' and internal address (localhost) filtering
            if ( net.family === 'IPv4' && !net.internal ) {
                return net.address;
            }
        }
    }
    return null; // Fallback if no external IPv4 is found
};

function kill ( tray, wss ) {
    tray.kill();
    wss.close();
}

async function getTargetAppWindow () {
    let rv = null;
    for ( const window of windowManager.getWindows() ) {
        if ( reContainsTargetApp.test( window.path ) ) {
            window.bringToTop();
            rv = window;
            break;
        }
    }

    if ( rv === null ) {
        console.warn( "No active window found." );
    }
    return rv;
}

function processMessage ( message ) {
    try {
        const command = JSON.parse( message );
        console.debug( "processMessage command:", command );
        sendKeyCombo( command );
    } catch ( e ) {
        console.error( "processMessage error from message:", e );
    }
}

async function sendKeyCombo ( command ) {
    try {
        const targetAppWindow = await getTargetAppWindow();
        if ( !targetAppWindow ) {
            TRAY.notify( appTitle, 'Is your target app running?' );
            return;
        }

        const modifiers = Array.isArray( command.modifiers )
            ? command.modifiers.filter( modifier => VALID_MODIFIERS.includes( modifier ) )
            : [];

        console.debug( "Sending", command.key, modifiers );
        robot.keyTap( command.key, modifiers );
        console.debug( "Sent command to target app." );
    }

    catch ( error ) {
        console.error( "Error sending key combination:", error );
    }
}

const httpServer = http.createServer( ( _req, res ) => {
    fs.readFile( path.join( DIST_DIR, 'index.html' ), ( err, data ) => {
        if ( err ) {
            console.error( err );
            res.statusCode = 404;
            res.setHeader( 'Content-Type', 'text/plain' );
            res.end( '404: Not Found' );
        } else {
            res.statusCode = 200;
            res.setHeader( 'Content-Type', 'text/html' );
            res.end( data );
        }
    } );
} );

dotenv.config();

const missingEnvVars = REQUIRED_ENV_VARS.filter( varName => !process.env[ varName ] );
if ( missingEnvVars.length > 0 ) {
    console.error( `Missing required environment variables: ${ missingEnvVars.join( ', ' ) }` );
    process.exit( 1 );
}

const appTitle = process.env.VITE_APP_TITLE || 'MyRemote';
const regexp = process.env.VITE_APP_RE;
const reContainsTargetApp = new RegExp( regexp, 'i' );
const wsPort = process.env.VITE_WS_PORT || 8223;
const httpPort = process.env.VITE_HTTP_PORT || 8224;
const wsAddress = 'ws://' + getLocalIP() + ':' + wsPort;
const httpAddressLink = 'http://' + getLocalIP() + ':' + httpPort + '?' + encodeURIComponent( wsAddress );

Tray.create(
    ( _tray ) => {
        TRAY = _tray;
        httpServer.listen( httpPort, '0.0.0.0', () => {
            console.info( `Server running at ${ httpAddressLink }/` );
        } );

        const wss = new WebSocketServer( {
            port: wsPort
        } );
        wss.on( 'connection', ( ws ) => {
            console.info( 'WS client connected' );
            // tray.notify( appTitle, 'Connected' );
            ws.on( 'message', processMessage );
            ws.on( 'close', () => {
                console.info( 'Client disconnected' );
            } );
        } );

        TRAY.setMenu(
            TRAY.item( appTitle, {
                bold: true,
                action: () => open( httpAddressLink )
            } ),
            TRAY.item( "Show", () => open( httpAddressLink ) ),
            TRAY.separator(),
            TRAY.item( "Quit", () => kill( TRAY, wss ) )
        );
        TRAY.setTitle( appTitle );
    }
);

