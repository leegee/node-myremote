import type { Command } from '../types/commands';
import defaultCommandsJson from '../../commands.json';

const LOCAL_STORAGE_KEY = 'remote_cmds';

export function setCommandsToDefault(): Command[] {
    saveCommands(defaultCommandsJson as Command[]);
    return defaultCommandsJson as Command[];
}

export function loadCommands() {
    console.log("Loading commands");
    let parsedCommands: Command[];
    try {
        const localCommandStr = localStorage.getItem(LOCAL_STORAGE_KEY) ?? undefined;
        parsedCommands = localCommandStr ? JSON.parse(localCommandStr) : defaultCommandsJson;
    } catch (e) {
        parsedCommands = [];
        console.error(e);
    }
    console.log("Loaded", parsedCommands);
    return parsedCommands;
};

export function saveCommands(commands: Command[]) {
    localStorage.setItem(LOCAL_STORAGE_KEY, JSON.stringify(commands));
}

export function downloadCommandsJson() {
    const commands = loadCommands();
    const blob = new Blob([JSON.stringify(commands, null, 2)], { type: 'application/json' });

    // Create a link element to trigger the download
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = 'commands.json';
    link.click();
}
