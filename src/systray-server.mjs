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

async function getCubaseWindow () {
    let rv = null;
    for ( const window of windowManager.getWindows() ) {
        if ( reContainsCubase.test( window.path ) ) {
            window.bringToTop();
            rv = window;
            break;
        }
    }
    return rv;
}

function processMessage ( message ) {
    console.log( "processMessage enter:", message );
    try {
        const command = JSON.parse( message );
        console.log( "processMessage command:", command );
        sendShortcut( command );
    } catch ( e ) {
        console.error( "processMessage error from message:", e );
    }
}

async function sendShortcut ( command ) {
    try {
        const cubaseWindow = await getCubaseWindow();
        if ( !cubaseWindow ) {
            console.log( "No active Cubase window found." );
            return;
        }

        const modifiers = command.modifiers instanceof Array ?
            command.modifiers.filter( 'control', 'shift', 'alt', 'command' ) : [];

        robot.keyTap( command.key, modifiers );

        console.log( "Sent command." );
    }

    catch ( error ) {
        console.error( "Error sending key combination:", error );
    }
}


dotenv.config();

const appTitle = process.env.VITE_APP_TITLE || 'MyRemote';
const reContainsCubase = /cubase/i;
const wsPort = process.env.VITE_WS_PORT || 8223;
const httpPort = process.env.VITE_HTTP_PORT || 8224;
const __filename = fileURLToPath( import.meta.url );
const __dirname = path.dirname( __filename );
const distDir = path.join( __dirname, '..', 'dist' );
const wsAddress = 'ws://' + getLocalIP() + ':' + wsPort;
const httpAddressLink = 'http://' + getLocalIP() + ':' + httpPort + '?' + encodeURIComponent( wsAddress );

console.log( httpAddressLink );

const httpServer = http.createServer( ( _req, res ) => {
    fs.readFile( path.join( distDir, 'index.html' ), ( err, data ) => {
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


Tray.create(
    ( tray ) => {
        httpServer.listen( httpPort, '0.0.0.0', () => {
            console.log( `Server running at ${ httpAddressLink }/` );
        } );

        const wss = new WebSocketServer( {
            port: wsPort
        } );
        wss.on( 'connection', ( ws ) => {
            console.log( 'WS client connected' );
            // tray.notify( appTitle, 'Connected' );
            ws.on( 'message', processMessage );
            ws.on( 'close', () => {
                console.log( 'Client disconnected' );
            } );
        } );

        tray.setMenu(
            tray.item( appTitle, {
                bold: true,
                action: () => open( httpAddressLink )
            } ),
            tray.item( "Show", open( httpAddressLink ) ),
            tray.separator(),
            tray.item( "Quit", () => kill( tray, wss ) )
        );
        tray.setTitle( appTitle );
    }
);

