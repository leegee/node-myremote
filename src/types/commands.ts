export type Modifier = 'control' | 'shift' | 'alt' | 'comamnd';

export interface Command {
    icon: string;
    text: string;
    color: string;
    key: string;
    modifiers?: Modifier[];
}
