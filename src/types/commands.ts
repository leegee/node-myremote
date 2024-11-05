export type Modifier = 'control' | 'shift' | 'alt' | 'command';

export interface Command {
    icon: string;
    text: string;
    color: string;
    key: string;
    modifiers?: Modifier[];
}
