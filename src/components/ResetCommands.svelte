<!-- ResetCommands -->
<script lang="ts">
  import { commandsStore } from "../stores/commandsStore";
  import { setCommandsToDefault } from "../lib/Commands";

  function handleResetToDefaults() {
    const defaultCommands = setCommandsToDefault();
    commandsStore.set(defaultCommands);
  }
</script>

<button on:click={handleResetToDefaults}> Reset to defaults. </button>
