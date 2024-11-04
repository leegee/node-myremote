<script lang="ts">
  import { onMount } from "svelte";
  import IconList from "./lib/IconList.svelte";

  let ws: WebSocket;
  let isConnected: boolean = false;
  let connectionError: string | null = null;

  function initWebSocket() {
    return new Promise<WebSocket>((resolve, reject) => {
      const url = "ws://localhost:" + import.meta.env.VITE_WS_PORT;
      const wsInstance = new WebSocket(url);
      console.info("Connecting to", url);

      wsInstance.onopen = () => {
        console.log("WebSocket connection opened");
        isConnected = true;
        resolve(wsInstance);
      };

      wsInstance.onmessage = (event) => {
        console.log(
          "Message from server:",
          JSON.stringify(event.data, null, 4)
        );
      };

      wsInstance.onclose = () => {
        console.log("WebSocket connection closed");
        isConnected = false;
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
  {#if isConnected}
    <IconList {ws} />
  {:else if connectionError}
    <p style="background-color: red; color: white; font-weight: bold">
      {connectionError}
    </p>
  {:else}
    <p>Connecting to WebSocket...</p>
  {/if}
</main>
