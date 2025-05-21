<!-- SaveCommandsToFile -->
<script lang="ts">
  import { downloadCommandsJson } from "../lib/Commands";
</script>

<button on:click={downloadCommandsJson}> Export </button>
