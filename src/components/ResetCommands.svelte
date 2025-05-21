<!-- ResetCommands -->
<script lang="ts">
  import { commandsStore } from "../stores/commandsStore";
  import { setCommandsToDefault } from "../lib/Commands";

  function handleResetToDefaults() {
    if (confirm("Reset all commands?")) {
      const defaultCommands = setCommandsToDefault();
      commandsStore.set(defaultCommands);
    }
  }
</script>

<button on:click={handleResetToDefaults}> Reset to defaults. </button>
