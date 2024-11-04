// https://robotjs.io/docs/syntax
import type { Command } from './types/commands';

export let commands: Command[] = [
    { label: "💾", alt: 'Save', color: "blue", key: "s", modifiers: ['control'] },
    { label: "●", alt: 'Record', color: "red", key: "r", modifiers: ['control'] },
    { label: "▶", alt: 'Play', color: "green", key: "space" },
    { label: "▮", alt: 'Pause', color: "orange", key: "numpad_0" },
    { label: "[", alt: 'Left locator', color: "orange", key: "home" },
    { label: "]", alt: 'Right locator', color: "orange", key: "end" },
];

export default commands;