// https://robotjs.io/docs/syntax
import type { Command } from './types/commands';

export let commands: Command[] = [
    { icon: "💾", text: 'Save', color: "blue", key: "s", modifiers: ['control'] },
    { icon: "🔴", text: 'Record', color: "red", key: "r", modifiers: ['control'] },
    { icon: "▶", text: 'Play', color: "green", key: "space" },
    { icon: "▮", text: 'Pause', color: "orange", key: "numpad_0" },
    { icon: "[", text: 'Left locator', color: "orange", key: "home" },
    { icon: "]", text: 'Right locator', color: "orange", key: "end" },
];

export default commands;