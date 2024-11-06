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
