import type { Command } from '../types/commands';
import commandsJson from '../../commands.json';

export function loadCommands() {
    let parsedCommands: Command[];
    try {
        parsedCommands = JSON.parse(localStorage.getItem("commands") ?? '[]') ?? commandsJson;
    } catch (e) {
        parsedCommands = [];
        console.error(e);
    }
    return parsedCommands;
};
