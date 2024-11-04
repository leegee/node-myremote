<script lang="ts">
    export let iconLabel = "Click me!";
    export let iconColor = "blue";
    export let iconSize = "64pt";
    export let ws: WebSocket; 

    function handleClick() {
        alert(`You clicked on ${iconLabel}`);
        
        // Send a WebSocket message
        if (ws && ws.readyState === WebSocket.OPEN) {
            ws.send(`Message from ${iconLabel}`);
        } else {
            console.error('WebSocket is not open. Message not sent.');
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

<div
    class="icon"
    style="--icon-color: {iconColor}; --icon-size: {iconSize};"
    on:click={handleClick}
>
    {iconLabel}
</div>
