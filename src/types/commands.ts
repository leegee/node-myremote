export type Modifier = 'control' | 'shift' | 'alt' | 'comamnd';

export interface Command {
    label: string;
    alt: string;
    color: string;
    key: string;
    modifiers?: Modifier[];
}
