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

const REQUIRED_ENV_VARS = ['VITE_WS_PORT', 'VITE_APP_TITLE', 'VITE_APP_RE'];
const VALID_MODIFIERS = ['control', 'shift', 'alt', 'command'];
const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const DIST_DIR = path.join(__dirname, '..', 'dist');

const APP_DATA_FOLDER =
    process.env.APPDATA || // Windows
    (process.platform === 'darwin' // maybe in future...
        ? path.join(os.homedir(), 'Library', 'Application Support') // macOS
        : path.join(os.homedir(), '.config')); // Linux

const MY_APP_FOLDER = path.join(APP_DATA_FOLDER, import.meta.env.VITE_APP_TITLE ?? "MyRemote");
if (!fs.existsSync(MY_APP_FOLDER)) {
    fs.mkdirSync(MY_APP_FOLDER, { recursive: true });
}
const customCustomConfigFilePath = Path.Combine(MY_APP_FOLDER, "custom-config.json");
const defaultCustomConfigFilePath = path.join(__dirname, '..', 'commands.json');

const getLocalIP = () => {
    const interfaces = os.networkInterfaces();
    for (const name of Object.keys(interfaces)) {
        for (const net of interfaces[name]) {
            if (net.family === 'IPv4' && !net.internal) {
                return net.address;
            }
        }
    }
    return null;
};

function kill(tray, wss) {
    tray.kill();
    wss.close();
}

async function getTargetAppWindow() {
    let rv = null;
    for (const window of windowManager.getWindows()) {
        if (reContainsTargetApp.test(window.path)) {
            window.bringToTop();
            rv = window;
            break;
        }
    }

    if (rv === null) {
        console.warn("No active window found.");
    }
    return rv;
}

function processMessage(jsonString) {
    try {
        const message = JSON.parse(jsonString);
        console.debug("processMessage command:", message);
        if (message.type) {
            processConfigMessage(message)
        } else {
            sendKeyCombo(message);
        }
    } catch (e) {
        console.error("processMessage error from message:", e);
    }
}

// Received type:'config', config:Config - write the file
function processConfigMessage(jsonConfigString) {
    fs.writeFileSync(customCustomConfigFilePath, jsonConfigString, 'utf-8');
    console.info('Wrote config to', customCustomConfigFilePath);
}

async function sendKeyCombo(keyMessage) {
    try {
        const targetAppWindow = await getTargetAppWindow();
        if (!targetAppWindow) {
            TRAY.notify(appTitle, 'Is your target app running?');
            return;
        }

        const modifiers = Array.isArray(keyMessage.modifiers)
            ? keyMessage.modifiers.filter(modifier => VALID_MODIFIERS.includes(modifier))
            : [];

        console.debug("Sending", keyMessage.key, modifiers);
        robot.keyTap(keyMessage.key, modifiers);
        console.debug("Sent command to target app.");
    }

    catch (error) {
        console.error("Error sending key combination:", error);
    }
}

const httpServer = http.createServer((_req, res) => {
    fs.readFile(path.join(DIST_DIR, 'index.html'), (err, data) => {
        if (err) {
            console.error(err);
            res.statusCode = 404;
            res.setHeader('Content-Type', 'text/plain');
            res.end('404: Not Found');
        } else {
            res.statusCode = 200;
            res.setHeader('Content-Type', 'text/html');
            res.end(data);
        }
    });
});

dotenv.config();

const missingEnvVars = REQUIRED_ENV_VARS.filter(varName => !process.env[varName]);
if (missingEnvVars.length > 0) {
    console.error(`Missing required environment variables: ${missingEnvVars.join(', ')}`);
    process.exit(1);
}

const appTitle = process.env.VITE_APP_TITLE || 'MyRemote';
const regexp = process.env.VITE_APP_RE;
const reContainsTargetApp = new RegExp(regexp, 'i');
const wsPort = process.env.VITE_WS_PORT || 8223;
const httpPort = process.env.VITE_HTTP_PORT || 8224;
const wsAddress = 'ws://' + getLocalIP() + ':' + wsPort;
const httpAddressLink = 'http://' + getLocalIP() + ':' + httpPort + '?' + encodeURIComponent(wsAddress);

Tray.create(
    (_tray) => {
        TRAY = _tray;
        httpServer.listen(httpPort, '0.0.0.0', () => {
            console.info(`HTTP server running at ${httpAddressLink}/`);
        });

        const wss = new WebSocketServer({
            port: wsPort
        });
        console.info(`WS server running on port ${wsPort}/`);

        wss.on('connection', (ws) => {
            console.info('WS client connected');
            // TRAY.notify( appTitle, 'Connected' );

            if (fs.existsSync(customCustomConfigFilePath)) {
                ws.send(fs.readFileSync(customCustomConfigFilePath, 'utf-8'));
            } else {
                ws.send(fs.readFileSync(defaultCustomConfigFilePath, 'utf-8'));
            }

            ws.on('message', processMessage);
            ws.on('close', () => {
                console.info('Client disconnected');
            });
        });

        TRAY.setMenu(
            TRAY.item(appTitle, {
                bold: true,
                action: () => open(httpAddressLink)
            }),
            TRAY.item("Show", () => open(httpAddressLink)),
            // should add the qr code here
            TRAY.separator(),
            TRAY.item("Quit", () => kill(TRAY, wss))
        );
        TRAY.setTitle(appTitle);
    }
);

