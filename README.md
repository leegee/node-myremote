# node-myremote

Exploring sending key commands to Cubase using scripting tools.

## Synopsis

    npm i                             # bun dislikes some deps
    bun run build
    node src/taskbar-server.mjs

## Description

Creates a Windows' system tray icon that can provide a URL for a webpage that 
can send key commands to Cubase.

This would work for any program, by updating a regexp.

    Sytem tray HTTP server -> HTTP document
    HTTP document -> System Tray WS server 
    System Tray WS server -> Cubase

## Resources

* https://www.utf8icons.com
* https://robotjs.io/docs/syntax