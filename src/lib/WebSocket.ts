import { commandsStore } from "../stores/commandsStore";
import type { Command } from "../types/commands";

type MessageType = {
    type: "config";
    commands: Command[];
};

export let wsInstance: WebSocket;
export let isConnected: boolean = false;
export let connectionError: string | null = null;
let WS_MAX_RECONNECT_ATTEMPTS = 100000;
let WS_RECONNECT_DELAY = 1000;

function initWebSocket() {
    let reconnectAttempts = 0;

    return new Promise<WebSocket>((resolve, reject) => {
        const url = decodeURIComponent(window.location.search.substring(1));
        console.info("Connecting to", url);
        const wsInstance = new WebSocket(url);

        wsInstance.onopen = () => {
            console.log("WebSocket connection opened");
            isConnected = true;
            resolve(wsInstance);
        };

        wsInstance.onmessage = (event) => {
            try {
                const message = JSON.parse(event.data);
                console.log("Message from server:" + message);
                processMessage(message);
            } catch (e) {
                console.error("Error decoding WS message:", e);
            }
        };

        wsInstance.onclose = () => {
            console.log("WebSocket connection closed");
            isConnected = false;
            // Retry
            if (reconnectAttempts < WS_MAX_RECONNECT_ATTEMPTS) {
                reconnectAttempts++;
                console.log(
                    `Attempting to reconnect... (${reconnectAttempts}/${WS_MAX_RECONNECT_ATTEMPTS})`,
                );
                setTimeout(() => {
                    setupWebSocket();
                }, WS_RECONNECT_DELAY);
            } else {
                connectionError = "Max reconnection attempts reached.";
            }
        };

        wsInstance.onerror = (error) => {
            console.error("WebSocket error:", error);
            reject("Failed to connect.");
        };
    });
}

export async function setupWebSocket() {
    try {
        wsInstance = await initWebSocket();
        connectionError = null;
    } catch (error) {
        console.error("Failed to connect WebSocket:", error);
        connectionError = error as string;
    }
}

function processMessage(message: MessageType) {
    if (message.type && message.type === "config") {
    }
}


commandsStore.subscribe((commands) => {
    if (wsInstance && wsInstance.readyState === WebSocket.OPEN) {
        wsInstance.send(
            JSON.stringify({
                type: "config",
                commands,
            }),
        );
    }
});
