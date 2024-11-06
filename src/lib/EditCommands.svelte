<script lang="ts">
  import { onDestroy } from "svelte";
  import type { Command, Modifier } from "../types/commands";
  import "./EditCommands.css";
  import { commandsStore } from "../stores/commandsStore";
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

  function deleteCommand(index: number) {
    if (confirm("Are you sure you want to delete this command?")) {
      commandsStore.update((currentCommands) =>
        currentCommands.filter((_, i) => i !== index)
      );
    }
  }

  function editCommand(index: number) {
    commandEdidintIndex = index;
  }

  function clearEditing() {
    commandEdidintIndex = null;
  }

  function handleDragStart(index: number) {
    draggedIndex = index;
    document.body.style.cursor = "move";
  }

  function handleDragEnd() {
    draggedIndex = null;
    document.body.style.cursor = "default";
  }

  function handleDragOver(event: DragEvent) {
    event.preventDefault(); // Required to allow dropping
  }

  function handleDrop(index: number) {
    if (draggedIndex !== null && draggedIndex !== index) {
      const newCommands = [...commands];
      const draggedCommand = newCommands[draggedIndex];
      newCommands.splice(draggedIndex, 1);
      newCommands.splice(index, 0, draggedCommand);
      commandsStore.set(newCommands);
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
          on:drop={() => handleDrop(index)}
        >
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
          </td>
          <td>
            <button on:click={() => deleteCommand(index)}>🗑</button>
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
