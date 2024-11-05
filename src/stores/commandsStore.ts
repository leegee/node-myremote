// commandsStore.ts
import { writable } from "svelte/store";
import { loadCommands, saveCommands } from "../lib/Commands";
import type { Command } from "../types/commands";

const initialCommands: Command[] = loadCommands();

export const commandsStore = writable<Command[]>(initialCommands);

commandsStore.subscribe((commands) => {
    saveCommands(commands);
});
