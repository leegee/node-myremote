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
      class:line-break={"linebreak" in command}
      draggable="true"
      on:dragstart={() => handleDragStart(index)}
      on:dragend={handleDragEnd}
      on:dragover={handleDragOver}
      on:drop={(e) => handleDrop(index, e)}
    >
      {#if "key" in command}
        <LargeIcon {command} {ws} />
      {/if}
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

  .line-break {
    width: 100%;
    height: 1pt;
    background-color: transparent;
    color: transparent;
    border: none;
    margin: 0.25em;
    display: block;
    content: "";
  }
</style>
