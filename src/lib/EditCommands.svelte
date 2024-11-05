<script lang="ts">
  import { onMount } from "svelte";

  import { loadCommands, saveCommands } from "./Commands";
  import type { Command, Modifier } from "../types/commands";

  let commands: Command[] = [];
  let icon = "";
  let text = "";
  let color = "";
  let key = "";
  let modifiers: Modifier[] = [];
  const commandsJson = "[]";

  onMount(() => (commands = loadCommands()));

  function addCommand() {
    if (icon && text && color && key) {
      const newCommand: Command = { icon, text, color, key, modifiers };
      commands.push(newCommand);
      saveCommands(commands);

      // Reset input fields
      icon = "";
      text = "";
      color = "";
      key = "";
      modifiers = [];
    } else {
      alert("Please fill in all fields!");
    }
  }
</script>

<div class="command-form">
  <input type="text" bind:value={icon} placeholder="Icon" />
  <input type="text" bind:value={text} placeholder="Text" />
  <input type="text" bind:value={color} placeholder="Color" />
  <input type="text" bind:value={key} placeholder="Key" />
  <input
    type="text"
    bind:value={modifiers}
    placeholder="Modifiers (comma separated)"
  />

  <button on:click={addCommand}>Add Command</button>
</div>

<style>
  .command-form {
    display: flex;
    flex-direction: column;
    gap: 10px;
  }
</style>
