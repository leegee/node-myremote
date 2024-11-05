<script lang="ts">
  import { onDestroy } from "svelte";
  import AddCommandRow from "./AddCommandRow.svelte";
  import { commandsStore } from "../stores/commandsStore";
  import "./EditCommands.css";
  import type { Command, Modifier } from "../types/commands";

  let commands: Command[] = [];
  const possibleModifiers: Modifier[] = ["shift", "control", "alt", "command"];

  // Subscribe to the commands store
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
      <tr>
        <td class="icon">{command.icon}</td>
        <td>{command.text}</td>
        <td
          ><span class="clr" style="background-color:{command.color}"
            >{command.color}</span
          ></td
        >
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
          <button on:click={() => deleteCommand(index)}>Delete</button>
        </td>
      </tr>
    {/each}
    <!-- Add Command Row -->
    <AddCommandRow {possibleModifiers} />
  </tbody>
</table>
