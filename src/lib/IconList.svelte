<script lang="ts">
  import LargeIcon from './LargeIcon.svelte';

  // props
  export let ws: WebSocket; 

  let icons = [
      { label: "Icon 1", color: "blue" },
      { label: "Icon 2", color: "green" },
      { label: "Icon 3", color: "red" }
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
  {#each icons as icon}
      <LargeIcon iconLabel={icon.label} iconColor={icon.color} {ws}/>
  {/each}
</div>
