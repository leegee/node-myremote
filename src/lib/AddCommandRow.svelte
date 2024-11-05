<script lang="ts">
  import { commandsStore } from "../stores/commandsStore";
  import "./EditCommands.css";
  import type { Command, Modifier } from "../types/commands";

  export let possibleModifiers: Modifier[];

  let icon = "";
  let text = "";
  let color = "";
  let key = "";
  let selectedModifiers: Modifier[] = [];

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

  function toggleModifier(modifier: Modifier) {
    if (selectedModifiers.includes(modifier)) {
      selectedModifiers = selectedModifiers.filter((m) => m !== modifier);
    } else {
      selectedModifiers = [...selectedModifiers, modifier];
    }
  }
</script>

<tr>
  <td class="centred icon">
    <input type="text" bind:value={icon} placeholder="Icon" />
  </td>
  <td><input type="text" bind:value={text} placeholder="Text" /></td>
  <td>
    <span class="clr">
      <input type="color" bind:value={color} />
    </span>
  </td>
  <td class="key">
    <input type="text" bind:value={key} placeholder="Key" />
  </td>
  {#each possibleModifiers as modifier}
    <td>
      <input
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
