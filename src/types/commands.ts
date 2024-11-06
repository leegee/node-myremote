export type Modifier = 'control' | 'shift' | 'alt' | 'command';

type linebreak = {
    linebreak: true;
};

type command = {
    icon: string;
    text: string;
    color: string;
    key: string;
    modifiers?: Modifier[];
};

export type Command = command | linebreak;
