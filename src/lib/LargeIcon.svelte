<script lang="ts">
    import type { Command } from '../types/commands';
    export let command: Command;
    export let ws: WebSocket; 

    function handleClick() {
        console.debug("You clicked on", command);
        
        if (ws && ws.readyState === WebSocket.OPEN) {
            ws.send(JSON.stringify(command));
        } else {
            console.error('WebSocket is not open. Comamnd not sent.');
        }
    }
</script>

<style>
    button {
        width: var(--icon-size, 100pt);
        height: var(--icon-size, 100pt);
        background-color: var(--icon-color, blue);
        font-size: calc( var(--icon-size) / 2);
        font-weight: 500;
        font-family: inherit;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        cursor: pointer;
        color: white;
        transition: background-color 0.3s;
        border-radius: 50%;
        border: 1px solid transparent;
        padding: 0.6em 1.2em;
        background-color: var(--icon-color);
        cursor: pointer;
        transition: border-color 0.25s;
    }

    button:hover {
        border-color: #646cff;
    }

    button:focus,
    button:focus-visible {
        outline: 4px auto -webkit-focus-ring-color;
    }

</style>

<!-- svelte-ignore a11y_click_events_have_key_events -->
<!-- svelte-ignore a11y_no_static_element_interactions -->
<button
    class="icon"
    style="--icon-color: {command.color}; --icon-size: 64pt;"
    on:click={handleClick}
>
    {command.label}
</button>
