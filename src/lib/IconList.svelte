<script lang="ts">
  import { onMount } from "svelte";
  import commands from "./Commands";
  import LargeIcon from "./LargeIcon.svelte";

  export let ws: WebSocket;

  let actualCommands = commands;

  onMount(() => {
    try {
      actualCommands =
        JSON.parse(localStorage.getItem("commands") ?? "null") || commands;
    } catch {
      actualCommands = commands;
    }
  });
</script>

<section class="icon-list">
  {#each actualCommands as command}
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
