<script lang="ts">
  import type { Command } from '../types/commands';
  import LargeIcon from './LargeIcon.svelte';

  // props
  export let ws: WebSocket;

  // https://robotjs.io/docs/syntax
  let commands: Command[] = [
      { label: "Save", color: "blue", key: "s", modifiers: ['CTRL'] },
      { label: "Record", color: "red", key: "r", modifiers: ['CTRL'] },
      { label: "Start/Stop", color: "green", key: "space" },
  ];
</script>

<style>
  .icon-list {
      display: flex;
      flex-direction: column;
      gap: 20px; 
  }
</style>

<div class="icon-list">
  {#each commands as command}
      <LargeIcon {command} {ws}/>
  {/each}
</div>
