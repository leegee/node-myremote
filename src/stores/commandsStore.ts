// commandsStore.ts
import { writable } from "svelte/store";
import { loadCommandsFromLocalStorage, saveCommandsToLocalStorage } from "../lib/Commands";
import type { Command } from "../types/commands";

const initialCommands: Command[] = loadCommandsFromLocalStorage();

export const commandsStore = writable<Command[]>(initialCommands);

commandsStore.subscribe((commands) => {
    saveCommandsToLocalStorage(commands);
});
