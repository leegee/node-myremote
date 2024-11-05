import type { Command } from '../types/commands';
import commandsJson from '../../commands.json';

const LOCAL_STORAGE_KEY = 'commands';

export function loadCommands() {
    console.log("Loading commands");
    let parsedCommands: Command[];
    try {
        const localCommandStr = localStorage.getItem(LOCAL_STORAGE_KEY) ?? undefined;
        parsedCommands = localCommandStr ? JSON.parse(localCommandStr) : commandsJson;
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
