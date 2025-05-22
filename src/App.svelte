<script lang="ts">
  import { onMount } from "svelte";
  import IconList from "./components/IconList.svelte";
  import EditCommands from "./components/CommandsTable.svelte";
  import Modal from "./components/Modal.svelte";
  import {
    connectionError,
    isConnected,
    setupWebSocket,
    wsInstance,
  } from "./lib/WebSocket";

  onMount(setupWebSocket);
</script>

<main>
  <nav class="button-container">
    <Modal buttonTitle="Edit" modalTitle="Edit Comamnds">
      <EditCommands />
    </Modal>
  </nav>

  {#if isConnected}
    <IconList ws={wsInstance} />
  {:else if connectionError}
    <p style="background-color: red; color: white; font-weight: bold">
      {connectionError}
    </p>
    <p>
      Is {import.meta.env.VITE_APP_TITLE || "MyRemote"} running on the same machine
      as the DAW?
    </p>
  {:else}
    <p>
      Connecting to the {import.meta.env.VITE_APP_TITLE || "MyRemote"} service...
    </p>
  {/if}
</main>

<style>
  .button-container {
    position: absolute;
    top: 0;
    right: 0;
  }
</style>
