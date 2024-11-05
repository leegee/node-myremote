<script lang="ts">
  import { onDestroy, onMount } from "svelte";

  import { commandsStore } from "../stores/commandsStore";
  import type { Command, Modifier } from "../types/commands";

  let commands: Command[] = [];
  let icon = "";
  let text = "";
  let color = "";
  let key = "";
  let modifiers: Modifier[] = [];

  const unsubscribe = commandsStore.subscribe((value) => {
    commands = value;
  });

  onDestroy(unsubscribe);

  function addCommand() {
    if (icon && text && color && key) {
      const newCommand: Command = { icon, text, color, key, modifiers };
      commandsStore.update((currentCommands) => [
        ...currentCommands,
        newCommand,
      ]);

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

  function deleteCommand(index: number) {
    if (confirm("Are you sure you want to delete this command?")) {
      commandsStore.update((currentCommands) =>
        currentCommands.filter((_, i) => i !== index)
      );
    }
  }
</script>

<table class="command-table">
  <thead>
    <tr>
      <th>Icon</th>
      <th>Text</th>
      <th>Color</th>
      <th>Key</th>
      <th>Modifiers</th>
      <th>Actions</th>
      <!-- New Actions Column -->
    </tr>
  </thead>
  <tbody>
    {#each commands as command, index}
      <tr>
        <td>{command.icon}</td>
        <td>{command.text}</td>
        <td>{command.color}</td>
        <td>{command.key}</td>
        <td>{command.modifiers ? command.modifiers.join(", ") : ""}</td>
        <td>
          <button on:click={() => deleteCommand(index)}>Delete</button>
          <!-- Delete Button -->
        </td>
      </tr>
    {/each}

    <tr>
      <td><input type="text" bind:value={icon} placeholder="Icon" /></td>
      <td><input type="text" bind:value={text} placeholder="Text" /></td>
      <td><input type="text" bind:value={color} placeholder="Color" /></td>
      <td><input type="text" bind:value={key} placeholder="Key" /></td>
      <td>
        <input
          type="text"
          bind:value={modifiers}
          placeholder="Modifiers (comma separated)"
        />
      </td>
      <td>
        <button class="add" on:click={addCommand}>Add</button>
      </td>
    </tr>
  </tbody>
</table>

<style>
  .command-table {
    width: 100%;
    border-collapse: collapse;
  }

  .command-table th,
  .command-table td {
    padding: 4pt;
    text-align: left;
    vertical-align: middle;
  }

  .command-table input {
    width: 100%;
  }

  .command-table button {
    background-color: #ff4d4d;
    color: white;
    border: none;
    border-radius: 2pt;
    cursor: pointer;
    padding: 2pt 6pt;
  }

  .command-table button.add {
    background-color: green;
  }
</style>
