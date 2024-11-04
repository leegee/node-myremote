<script lang="ts">
  import commands from '../commands';
  import LargeIcon from './LargeIcon.svelte';

  export let ws: WebSocket;
</script>

<section class="icon-list">
  {#each commands as command}
      <LargeIcon {command} {ws}/>
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

