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
    .icon {
        width: var(--icon-size, 100px);
        height: var(--icon-size, 100px);
        background-color: var(--icon-color, blue);
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        cursor: pointer;
        font-size: 24px;
        color: white;
        transition: background-color 0.3s;
    }
    
    .icon:hover {
        background-color: darkblue;
    }
</style>

<!-- svelte-ignore a11y_click_events_have_key_events -->
<!-- svelte-ignore a11y_no_static_element_interactions -->
<div
    class="icon"
    style="--icon-color: {command.color}; --icon-size: 64pt;"
    on:click={handleClick}
>
    {command.label}
</div>
