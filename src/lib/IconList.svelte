<script lang="ts">
  import { onDestroy } from "svelte";
  import LargeIcon from "./LargeIcon.svelte";
  import { commandsStore } from "../stores/commandsStore";
  import type { Command } from "../types/commands";
  import { setupDragAndDrop } from "./dragDrop";

  export let ws: WebSocket;

  let commands: Command[] = [];

  const unsubscribe = commandsStore.subscribe((value) => {
    commands = value;
  });

  onDestroy(unsubscribe);

  const { handleDragStart, handleDragOver, handleDrop, handleDragEnd } =
    setupDragAndDrop(commands, commandsStore);
</script>

<section class="icon-list">
  {#each commands as command, index}
    <!-- svelte-ignore a11y_no_static_element_interactions -->
    <div
      draggable="true"
      on:dragstart={() => handleDragStart(index)}
      on:dragend={handleDragEnd}
      on:dragover={handleDragOver}
      on:drop={(e) => handleDrop(index, e)}
    >
      <LargeIcon {command} {ws} />
    </div>
  {/each}
</section>

<style>
  .icon-list {
    display: flex;
    flex-direction: row;
    gap: 10pt;
    flex-wrap: wrap;
    align-content: center;
    justify-content: space-around;
    align-items: center;
  }

  /* .dragging {
    opacity: 0.5;
  } */
</style>
