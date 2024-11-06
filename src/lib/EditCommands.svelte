<!-- EditCommands.svelte -->
<script lang="ts">
  import { onDestroy } from "svelte";
  import { commandsStore } from "../stores/commandsStore";
  import AddOrEditCommandRow from "./AddOrEditCommandRow.svelte";
  import type { Command, Modifier } from "../types/commands";
  import "./EditCommands.css";
  import { downloadCommandsJson, setCommandsToDefault } from "./Commands";

  let commands: Command[] = [];
  const possibleModifiers: Modifier[] = ["shift", "control", "alt", "command"];
  let isEditingIndex: number | null = null; // Store the index of the command being edited

  const unsubscribe = commandsStore.subscribe((value) => {
    commands = value;
  });

  onDestroy(unsubscribe);

  function deleteCommand(index: number) {
    if (confirm("Are you sure you want to delete this command?")) {
      commandsStore.update((currentCommands) =>
        currentCommands.filter((_, i) => i !== index)
      );
    }
  }

  function editCommand(index: number) {
    isEditingIndex = index;
  }

  function clearEditing() {
    isEditingIndex = null;
  }

  function handleResetToDefaults() {
    const defaultCommands = setCommandsToDefault();
    commandsStore.set(defaultCommands);
  }
</script>

<table class="command-table">
  <thead>
    <tr>
      <th>Icon</th>
      <th>Text</th>
      <th>Color</th>
      <th>Key</th>
      {#each possibleModifiers as modifier}
        <th class="modifier"> {modifier} </th>
      {/each}
      <th>Actions</th>
    </tr>
  </thead>
  <tbody>
    {#each commands as command, index}
      {#if isEditingIndex === index}
        <!-- Render the editor for the command being edited -->
        <AddOrEditCommandRow
          {possibleModifiers}
          {command}
          on:cancel={clearEditing}
        />
      {:else}
        <tr>
          <td class="icon">{command.icon}</td>
          <td>{command.text}</td>
          <td>
            <span class="clr" style="background-color:{command.color}"
              >{command.color}</span
            >
          </td>
          <td class="key">{command.key}</td>
          {#each possibleModifiers as modifier}
            <td class="modifier">
              <input
                type="checkbox"
                checked={command.modifiers?.includes(modifier)}
                disabled
              />
            </td>
          {/each}
          <td>
            <button on:click={() => editCommand(index)}>🖉</button>
            <button on:click={() => deleteCommand(index)}>🗑</button>
          </td>
        </tr>
      {/if}
    {/each}

    <!-- Add Command Row -->
    <AddOrEditCommandRow
      {possibleModifiers}
      command={null}
      on:cancel={clearEditing}
    />
  </tbody>
</table>

<nav class="toolbar">
  <button on:click={handleResetToDefaults}> Reset to defaults. </button>

  <button on:click={downloadCommandsJson}>Download</button>

  <!-- <button on:click={downloadCommandsJson}>Load</button> -->
</nav>
