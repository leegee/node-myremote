<!-- AddOrEditCommandRow.svelte -->
<script lang="ts">
  import { createEventDispatcher } from "svelte";
  import { commandsStore } from "../stores/commandsStore";
  import type { Command, Modifier } from "../types/commands";
  import "./EditCommands.css";

  export let possibleModifiers: Modifier[];
  export let command: Command | null = null; // Command to edit
  const dispatch = createEventDispatcher();

  // Initialize input fields
  let icon = "";
  let text = "";
  let color = "";
  let key = "";
  let selectedModifiers: Modifier[] = [];

  // If there's a command to edit, populate fields
  if (command && "key" in command) {
    icon = command.icon;
    text = command.text;
    color = command.color;
    key = command.key;
    selectedModifiers = command.modifiers || [];
  }

  function saveCommand() {
    if (icon && text && color && key) {
      const updatedCommand: Command = {
        icon,
        text,
        color,
        key,
        modifiers: selectedModifiers,
      };

      if (command) {
        // Update existing command
        commandsStore.update((currentCommands) => {
          const index = currentCommands.indexOf(command);
          if (index !== -1) {
            currentCommands[index] = updatedCommand;
          }
          return [...currentCommands];
        });
      } else {
        // Add new command
        commandsStore.update((currentCommands) => [
          ...currentCommands,
          updatedCommand,
        ]);
      }

      // Reset input fields
      resetFields();
    } else {
      alert("Please fill in all fields!");
    }
  }

  function resetFields() {
    icon = "";
    text = "";
    color = "";
    key = "";
    selectedModifiers = [];
  }

  function toggleModifier(modifier: Modifier) {
    if (selectedModifiers.includes(modifier)) {
      selectedModifiers = selectedModifiers.filter((m) => m !== modifier);
    } else {
      selectedModifiers = [...selectedModifiers, modifier];
    }
  }

  function cancelEdit() {
    dispatch("cancel");
  }
</script>

<tr>
  <td class="centred icon">
    <input type="text" bind:value={icon} placeholder="Icon" />
  </td>
  <td>
    <input type="text" bind:value={text} placeholder="Text" />
  </td>
  <td>
    <input type="color" bind:value={color} />
  </td>
  <td class="key">
    <input type="text" bind:value={key} placeholder="Key" />
  </td>
  {#each possibleModifiers as modifier}
    <td class="modifier">
      <input
        type="checkbox"
        checked={selectedModifiers.includes(modifier)}
        on:change={() => toggleModifier(modifier)}
      />
    </td>
  {/each}
  <td>
    <button class="add" on:click={saveCommand}>
      {command ? "✔" : "🞥"}
    </button>
  </td>
  <td>
    <button on:click={cancelEdit}>🗙</button>
  </td>
</tr>
