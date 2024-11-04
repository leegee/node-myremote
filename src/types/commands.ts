export type Modifier = 'CTRL' | 'SHIFT' | 'ALT';

export interface Command {
    label: string;
    color: string;
    key: string;
    modifiers?: Modifier[];
}
