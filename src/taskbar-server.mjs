import { WebSocketServer } from 'ws';
import Tray from 'trayicon';
import robot from 'robotjs';
import { windowManager } from 'node-window-manager';
import dotenv from 'dotenv';

dotenv.config();

const title = 'MyRemote';
const reContainsCubase = /^cubase/i;

const wss = new WebSocketServer( { port: process.env.VITE_WS_PORT } );

wss.on( 'connection', ( ws ) => {
    console.log( 'Client connected' );

    ws.on( 'message', processMessage );

    ws.on( 'close', () => {
        console.log( 'Client disconnected' );
    } );
} );

Tray.create( function ( tray ) {
    // tray.setIcon( 'icon.png' );
    let quit = tray.item( "Quit " + title, () => kill( tray ) );
    tray.setMenu( quit );
} );

function kill ( tray ) {
    tray.kill();
    wss.close();
}

async function getCubaseWindow () {
    for ( const window of windowManager.getWindows() ) {
        if ( reContainsCubase.test( window.path ) ) {
            console.log( "Found Cubase window:", window );
            window.bringToTop();
            return window;
        }
    }
    return null;
}

async function sendShortcut () {
    try {
        const cubaseWindow = await getCubaseWindow();
        if ( !cubaseWindow ) {
            console.log( "No active Cubase window found." );
            return;
        }

        robot.keyToggle( 'control', 'down' );
        robot.keyTap( 's' );
        robot.keyToggle( 'control', 'up' );

        console.log( "Sent Ctrl + S to Cubase." );
    } catch ( error ) {
        console.error( "Error sending key combination:", error );
    }
}

function processMessage ( message ) {
    console.log( `Received message: ${ message }` );
}
