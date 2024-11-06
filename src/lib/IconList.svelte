<script lang="ts">
  import { onDestroy } from "svelte";
  import LargeIcon from "./LargeIcon.svelte";
  import { commandsStore } from "../stores/commandsStore";
  import type { Command } from "../types/commands";

  export let ws: WebSocket;

  let commands: Command[];
  let draggedIndex: number | null = null;

  const unsubscribe = commandsStore.subscribe((value) => {
    commands = value;
  });

  onDestroy(unsubscribe);

  // Handle the start of drag event
  function handleDragStart(index: number) {
    draggedIndex = index;
    document.body.style.cursor = "move"; // Optional, gives feedback when dragging
  }

  // Handle the end of drag event
  function handleDragEnd() {
    draggedIndex = null;
    document.body.style.cursor = "default";
  }

  // Handle drag over to allow dropping
  function handleDragOver(event: DragEvent) {
    event.preventDefault(); // Required to allow dropping
  }

  // Handle the drop event to reorder the items
  function handleDrop(index: number) {
    if (draggedIndex !== null && draggedIndex !== index) {
      const newCommands = [...commands];
      const draggedCommand = newCommands[draggedIndex];
      newCommands.splice(draggedIndex, 1);
      newCommands.splice(index, 0, draggedCommand);

      // Update the store with the new command order
      commandsStore.set(newCommands);
    }
  }
</script>

<section class="icon-list">
  {#each commands as command, index}
    <!-- svelte-ignore a11y_no_static_element_interactions -->
    <div
      draggable="true"
      on:dragstart={() => handleDragStart(index)}
      on:dragend={handleDragEnd}
      on:dragover={handleDragOver}
      on:drop={() => handleDrop(index)}
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

  /* Optional: Highlight the icon being dragged */
  .dragging {
    opacity: 0.5;
  }
</style>
