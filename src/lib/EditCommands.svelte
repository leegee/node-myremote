<script lang="ts">
  import { onDestroy } from "svelte";
  import type { Command, Modifier } from "../types/commands";
  import "./EditCommands.css";
  import { commandsStore } from "../stores/commandsStore";
  import { setupDragAndDrop } from "./dragDrop";
  import AddOrEditCommandRow from "./AddOrEditCommandRow.svelte";
  import LoadCommandsFromFile from "./LoadCommandsFromFile.svelte";
  import SaveCommandsToFile from "./SaveCommandsToFile.svelte";
  import ResetCommands from "./ResetCommands.svelte";

  let commands: Command[] = [];
  let draggedIndex: number | null = null;
  let commandEdidintIndex: number | null = null;
  const possibleModifiers: Modifier[] = ["shift", "control", "alt", "command"];

  const unsubscribe = commandsStore.subscribe((value) => {
    commands = value;
  });

  onDestroy(unsubscribe);

  const { handleDragStart, handleDragOver, handleDrop, handleDragEnd } =
    setupDragAndDrop(commands, commandsStore);

  function deleteCommand(index: number) {
    if (confirm("Are you sure you want to delete this command?")) {
      commandsStore.update((currentCommands) =>
        currentCommands.filter((_, i) => i !== index)
      );
    }
  }

  function insertLinebreak(index: number) {
    commandsStore.update((currentCommands) => {
      const newCommands = [...currentCommands];
      newCommands.splice(index + 1, 0, { linebreak: true });
      return newCommands;
    });
  }

  function editCommand(index: number) {
    commandEdidintIndex = index;
  }

  function clearEditing() {
    commandEdidintIndex = null;
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
      <th class="modifier">Edit</th>
      <th class="modifier">Delete</th>
    </tr>
  </thead>
  <tbody>
    {#each commands as command, index}
      {#if commandEdidintIndex === index}
        <AddOrEditCommandRow
          {possibleModifiers}
          {command}
          on:cancel={clearEditing}
        />
      {:else}
        <tr
          draggable="true"
          on:dragstart={() => handleDragStart(index)}
          on:dragend={handleDragEnd}
          on:dragover={handleDragOver}
          on:drop={(e) => handleDrop(index, e)}
        >
          {#if "linebreak" in command}
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            {#each possibleModifiers as modifier}
              <td></td>
            {/each}
          {:else}
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
          {/if}
          <td>
            <button on:click={() => editCommand(index)}>🖉</button>
          </td>
          <td>
            <button on:click={() => deleteCommand(index)}>🗑</button>
          </td>
          <td>
            <button on:click={() => insertLinebreak(index)}>↲</button>
          </td>
        </tr>
      {/if}
    {/each}

    <AddOrEditCommandRow
      {possibleModifiers}
      command={null}
      on:cancel={clearEditing}
    />
  </tbody>
</table>

<nav class="toolbar">
  <ResetCommands />
  <SaveCommandsToFile />
  <LoadCommandsFromFile />
</nav>
