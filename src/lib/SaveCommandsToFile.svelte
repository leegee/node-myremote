<!-- SaveCommandsToFile -->
<script lang="ts">
  import { downloadCommandsJson } from "./Commands";
</script>

<button on:click={downloadCommandsJson}> Save </button>
