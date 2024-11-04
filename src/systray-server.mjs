import { WebSocketServer } from 'ws';
import Tray from 'trayicon';
import robot from 'robotjs';
import { windowManager } from 'node-window-manager';
import dotenv from 'dotenv';

dotenv.config();

const appTitle = process.env.VITE_APP_TITLE || 'MyRemote';
const reContainsCubase = /cubase/i;

function kill ( tray ) {
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


Tray.create(
    ( tray ) => {
        const quit = tray.item( "Quit " + appTitle, () => kill( tray ) );
        tray.setMenu( quit );
        tray.setTitle( appTitle );

        const wss = new WebSocketServer( { port: process.env.VITE_WS_PORT || 8223 } );
        wss.on( 'connection', ( ws ) => {
            console.log( 'Client connected' );
            tray.notify( appTitle, 'Connected' );
            ws.on( 'message', processMessage );
            ws.on( 'close', () => {
                console.log( 'Client disconnected' );
            } );
        } );

    }
);

