<script lang="ts">
  import { onMount } from "svelte";
  import { loadCommands } from "./Commands";
  import LargeIcon from "./LargeIcon.svelte";
  import type { Command } from "../types/commands";

  export let ws: WebSocket;

  let commands: Command[];

  onMount(() => {
    commands = loadCommands();
  });
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
