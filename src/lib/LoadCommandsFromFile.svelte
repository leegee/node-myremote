<!-- LoadCommandsFromFile.svelte -->

<script lang="ts">
  import { commandsStore } from "../stores/commandsStore";
  import type { Command } from "../types/commands";
  import { saveCommands } from "./Commands";

  function loadCommandsFromFile(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input?.files?.[0];

    if (!file) {
      console.error("No file selected");
      return;
    }

    const reader = new FileReader();
    reader.onload = (e) => {
      try {
        const content = e.target?.result as string;
        const parsedCommands: Command[] = JSON.parse(content);

        saveCommands(parsedCommands);
        commandsStore.set(parsedCommands);
        console.log("Commands loaded:", parsedCommands);
      } catch (error) {
        console.error("Error loading file:", error);
      }
    };

    reader.readAsText(file);
  }
</script>

<!-- Hidden file input to trigger file selection -->
<input
  type="file"
  id="commandsFileInput"
  accept=".json"
  style="display: none"
  on:change={loadCommandsFromFile}
/>

<button on:click={() => document.getElementById("commandsFileInput")?.click()}>
  Load
</button>
