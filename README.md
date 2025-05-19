# node-myremote

Remote control Cubase from a mobile device/web page.

You could actually control any Windows application that accepts keyboard input, ymmv.

## Synopsis

    # bun dislikes some deps
    npm i                             
    
    # For the dotnet version:
    bun run build:dotnet
    bun run start:dotnet

    # For the JS version:
    bun run build
    bun run start

In both cases, the system tray (by the clock) will now show a dark icon for MyRemote -
right-click and either select to scan the QR code, or select Open to view MyRemote in
your web browser so you can copy the address to your mobile device.

*For security reasons, your Cubase must be in the foreground to receive commands.*
    
Or use the [Windows installer](./dotnet/MyRemote/Output/Install-MyRemote.exe).

System tray:

![System Tray](./README/taskbar.png)

Main interface:

![Main](./README/main.png)

Editor:

![Editor](./README/editor.png)

Editing:

![Editing](./README/editing.png)

## Description

Creates a Windows' system tray icon that can provide a URL for a webpage that 
can send key commands to Cubase, or any other Windows application.

Click 'edit' in the top right-hand corner to add, edited, and re-arranged buttons - 
changes are preserved between page loads, and can also be saved to and loaded 
from file.

Remember to check/update your Cubase key commands!

## Programmes Other Than Cubase

Configure the regex in the `.env` file.

## Versions

There is a Svelte Typescript version, and a dotnet version.

The former relies upon `robot.js` and thus a binary compiled for your specific 
version of Node.

The latter requires elevated privelages to listen to all network interfaces.
This is performed by the installer if you use it:

    # As admin:
    netsh http add urlacl url=http://+:8223/ user=Everyone
    netsh http add urlacl url=http://+:8224/ user=Everyone
    netsh advfirewall firewall add rule name="MyRemote" protocol=TCP dir=in localport=8223 action=allow
    netsh advfirewall firewall add rule name="MyRemote" protocol=TCP dir=in localport=8224 action=allow

## Program Flow

    Build: vite -> HTML/JS/CSS bundle
    Sytem tray HTTP server -> HTML/JS/CSS bundle
    HTML/JS/CSS bundle -> System Tray WS server 
    System Tray WS server -> Key combination -> Target App

## Build

    bun run build:dotnet

That  will build the vite bundle, copy assets to the dotnet directory,
run the dotnet release build, after which a tar ball can be created:

    bun run bundle

## Svelte Dev

    bun run start  # Starts the robot-js and outputs a URL
    bun run dev    # Starts the Vite dev server 

`start` will output the remote's full URL to the console.

## Future Work

* For dev, have Vite open the full URL
* Parse a Cubase XML
* Expose the regex used to select Cubase
* Maybe add options to launch programs/do other things on the host PC

## Resources

* https://www.utf8icons.com
* https://robotjs.io/docs/syntax
