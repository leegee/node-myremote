import type { Command } from '../types/commands';
import commandsJson from '../../commands.json';

let commands: Command[] = commandsJson;

console.log("Commands loaded:", commands);

export default commands;
