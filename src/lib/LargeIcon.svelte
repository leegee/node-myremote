<script lang="ts">
  import type { Command } from "../types/commands";
  export let command: Command;
  export let ws: WebSocket;

  function handleClick() {
    console.debug("You clicked on", command);

    if (ws && ws.readyState === WebSocket.OPEN) {
      ws.send(JSON.stringify(command));
    } else {
      console.error("WebSocket is not open. Command not sent.");
    }
  }
</script>

<button
  title={command.text}
  class="icon"
  style="--icon-color: {command.color}; --icon-size: 64pt;"
  on:click={handleClick}
>
  <div class="icon">{command.icon}</div>
  <div class="text">{command.text}</div>
</button>

<style>
  button {
    padding: 1em;
  }

  button .icon {
    width: var(--icon-size, 100pt);
    height: var(--icon-size, 100pt);
    font-size: calc(var(--icon-size) / 1.8);
    background-color: var(--icon-color, blue);
    line-height: 1;
    color: white;
    font-weight: 500;
    font-family: inherit;
    text-align: center;
    display: flex;
    align-items: center;
    justify-content: center;
    align-content: stretch;
    border-radius: 50%;
    border: 1pt solid transparent;
    padding: 0;
    padding-bottom: 0.15em;
    cursor: pointer;
    transition:
      border-color 0.25s,
      background-color 0.3s;
  }

  button:hover {
    border-color: #646cff;
  }

  button:focus,
  button:focus-visible {
    outline: 4px auto -webkit-focus-ring-color;
  }

  button .text {
    margin-top: 1em;
  }
</style>
