# node-myremote

Exploring sending shortcuts to Cubase using scripting tools.

    npm i                             # bun dislikes some deps
    node src/taskbar-server.mjs
    bun run dev


    {
      "label": "string",         // Human-readible 
      "key": "string",           // The key to be pressed (e.g., "s" for save)
      "modifiers": [             // An array of any modifier keys to be used
        "CTRL", 
        "ALT",
        "SHIFT"
      ]
    }
    


