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
    let link: HTMLAnchorElement | null = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = 'commands.json';
    link.click();
    link = null;
}


export function loadCommandsFromFile(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input?.files?.[0];

    if (!file) {
        console.warn("No file selected");
        return;
    }

    const reader = new FileReader();
    reader.onload = (e) => {
        try {
            const content = e.target?.result as string;
            const parsedCommands: Command[] = JSON.parse(content);
            saveCommands(parsedCommands);
            console.log("Commands loaded:", parsedCommands);
        } catch (error) {
            console.error("Error loading file:", error);
        }
    };

    reader.readAsText(file);
}
