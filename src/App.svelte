<script lang="ts">
  import { onMount } from "svelte";
  import IconList from "./components/IconList.svelte";
  import EditCommands from "./components/CommandsTable.svelte";
  import Modal from "./components/Modal.svelte";

  let ws: WebSocket;
  let isConnected: boolean = false;
  let connectionError: string | null = null;
  let WS_MAX_RECONNECT_ATTEMPTS = 100000;
  let WS_RECONNECT_DELAY = 1000;

  function initWebSocket() {
    let reconnectAttempts = 0;

    return new Promise<WebSocket>((resolve, reject) => {
      const url = decodeURIComponent(window.location.search.substring(1));
      console.info("Connecting to", url);
      const wsInstance = new WebSocket(url);

      wsInstance.onopen = () => {
        console.log("WebSocket connection opened");
        isConnected = true;
        resolve(wsInstance);
      };

      // noop
      wsInstance.onmessage = (event) => {
        console.log(
          "Message from server:",
          JSON.stringify(event.data, null, 4)
        );
      };

      wsInstance.onclose = () => {
        console.log("WebSocket connection closed");
        isConnected = false;
        // Retry
        if (reconnectAttempts < WS_MAX_RECONNECT_ATTEMPTS) {
          reconnectAttempts++;
          console.log(
            `Attempting to reconnect... (${reconnectAttempts}/${WS_MAX_RECONNECT_ATTEMPTS})`
          );
          setTimeout(() => {
            setupWebSocket();
          }, WS_RECONNECT_DELAY);
        } else {
          connectionError = "Max reconnection attempts reached.";
        }
      };

      wsInstance.onerror = (error) => {
        console.error("WebSocket error:", error);
        reject("Failed to connect.");
      };
    });
  }

  async function setupWebSocket() {
    try {
      ws = await initWebSocket();
      connectionError = null;
    } catch (error) {
      console.error("Failed to connect WebSocket:", error);
      connectionError = error as string;
    }
  }

  function setFullScreen() {
    if (!document.fullscreenElement) {
      document.documentElement.requestFullscreen();
    }
    window.removeEventListener("click", setFullScreen);
  }

  onMount(() => {
    setupWebSocket();
    window.addEventListener("click", setFullScreen);
  });
</script>

<main>
  <nav class="button-container">
    <Modal buttonTitle="Edit" modalTitle="Edit Comamnds">
      <EditCommands />
    </Modal>
  </nav>

  {#if isConnected}
    <IconList {ws} />
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
      Connecting to {import.meta.env.VITE_APP_TITLE || "MyRemote"} service that should
      be running on the same machine as the DAW...
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
