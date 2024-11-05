<script lang="ts">
  import { onDestroy } from "svelte";
  import LargeIcon from "./LargeIcon.svelte";
  import { commandsStore } from "../stores/commandsStore";
  import type { Command } from "../types/commands";

  export let ws: WebSocket;

  let commands: Command[];

  const unsubscribe = commandsStore.subscribe((value) => {
    commands = value;
  });

  onDestroy(unsubscribe);
</script>

<section class="icon-list">
  {#each commands as command}
    <LargeIcon {command} {ws} />
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
</style>
