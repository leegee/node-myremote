import Tray from 'trayicon';
import robot from 'robotjs';
import { windowManager } from 'node-window-manager';

const reContainsCubase = /^cubase/i;

// Create the tray icon
Tray.create( function ( tray ) {
    let main = tray.item( "Power" );

    main.add( tray.item( "Send Ctrl+S to Cubase", async () => {
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
    } ) );

    let quit = tray.item( "Quit", () => tray.kill() );
    tray.setMenu( main, quit );
} );

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

