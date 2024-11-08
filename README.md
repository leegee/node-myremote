# node-myremote

Remote control Windows applications from a mobile device/web page.

## Synopsis

    npm i                             # bun dislikes some deps
    
    # For TS:
    bun run build
    bun run start
    
    # For binary:
    bun run build:dotnet
    bun run start:dotnet

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

Key commands can be added and edited, are preserved between page loads, 
and can also be saved to and loaded from file.

To re-arrange commands, drag the icons or table rows.

This would work for any program, by updating the regex in the `.env`.

## Versions

There is a Typescript version, and a dotnet version. The latter requires 
elevated privelages to listen to all network interfaces:

    # As admin:
    netsh http add urlacl url=http://+:8223/ user=Everyone
    netsh http add urlacl url=http://+:8224/ user=Everyone

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

## Dev

Use the Vite dev server, make sure to append a query string with an encoded URI
to the web socket server: the sort of thing output by [the Node app prototype](src/systray-server.mjs).

## Future Work

* Parse a Cubase XML
* Expose the regex used to select the program
* Maybe add options to launch programs

## Resources

* https://www.utf8icons.com
* https://robotjs.io/docs/syntax