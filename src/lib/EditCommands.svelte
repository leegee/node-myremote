<script lang="ts">
  import { onDestroy } from "svelte";
  import { commandsStore } from "../stores/commandsStore";
  import type { Command, Modifier } from "../types/commands";
  import "./EditCommands.css";

  let commands: Command[] = [];
  let icon = "";
  let text = "";
  let color = "";
  let key = "";
  let selectedModifiers: Modifier[] = [];

  // List of possible modifier options
  const possibleModifiers: Modifier[] = ["shift", "control", "alt", "command"];

  // Subscribe to commandsStore
  const unsubscribe = commandsStore.subscribe((value) => {
    commands = value;
  });

  onDestroy(unsubscribe);

  function addCommand() {
    if (icon && text && color && key) {
      const newCommand: Command = {
        icon,
        text,
        color,
        key,
        modifiers: selectedModifiers,
      };
      commandsStore.update((currentCommands) => [
        ...currentCommands,
        newCommand,
      ]);

      // Reset input fields
      icon = "";
      text = "";
      color = "";
      key = "";
      selectedModifiers = [];
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

  function toggleModifier(modifier: Modifier) {
    if (selectedModifiers.includes(modifier)) {
      selectedModifiers = selectedModifiers.filter((m) => m !== modifier);
    } else {
      selectedModifiers = [...selectedModifiers, modifier];
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
        <td>
          <span class="clr" style="background-color:{command.color}"
            >{command.color}</span
          >
        </td>
        <td class="key">{command.key}</td>
        {#each possibleModifiers as modifier}
          <td class="modifier">
            <input
              title={modifier}
              type="checkbox"
              checked={command.modifiers &&
                command.modifiers.includes(modifier)}
              disabled
            />
          </td>
        {/each}
        <td>
          <button on:click={() => deleteCommand(index)}>Delete</button>
        </td>
      </tr>
    {/each}

    <tr>
      <td class="centred icon">
        <input class="icon" type="text" bind:value={icon} placeholder="Icon" />
      </td>
      <td><input type="text" bind:value={text} placeholder="Text" /></td>
      <td>
        <span class="clr">
          <input type="color" bind:value={color} placeholder="Color" />
        </span>
      </td>
      <td class="key"
        ><input type="text" bind:value={key} placeholder="Key" /></td
      >
      {#each possibleModifiers as modifier}
        <td class="modifier">
          <input
            title={modifier}
            type="checkbox"
            checked={selectedModifiers.includes(modifier)}
            on:change={() => toggleModifier(modifier)}
          />
        </td>
      {/each}
      <td class="centred">
        <button class="add" on:click={addCommand}>Add</button>
      </td>
    </tr>
  </tbody>
</table>
