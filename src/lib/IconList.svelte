<script lang="ts">
  import commands from '../commands';
  import LargeIcon from './LargeIcon.svelte';

  export let ws: WebSocket;
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
