<script lang="ts">
  export let buttonTitle: string = "Open";
  export let modalTitle: string = "Modal Title";

  let isOpen = false;

  function openModal() {
    isOpen = true;
  }

  function closeModal() {
    isOpen = false;
  }
</script>

<button on:click={openModal}>{buttonTitle}</button>

{#if isOpen}
  <dialog open class="modal">
    <header>
      <h2>
        <button on:click={closeModal} aria-label="Back" title="Back">﹤</button>
        {modalTitle}
      </h2>
    </header>
    <div class="modal-content">
      <slot></slot>
    </div>
  </dialog>
{/if}

<style>
  dialog {
    position: fixed;
    top: 0;
    left: 0;
    width: calc(100vw - 2em);
    padding: 2em;
    height: 100vh;
    border: none;
    border-radius: 8px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
  }

  header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding-bottom: 0.5em;
    margin-bottom: 0.5em;
    width: calc(100vw - 6em);
  }

  h2 {
    font-size: 1.2em;
    margin: 0;
  }

  button[aria-label="Back"] {
    background: none;
    border: none;
    padding: 0;
    margin: 0;
    margin-right: 0.25em;
    cursor: pointer;
    font-weight: bolder;
  }

  .modal-content {
    padding: 1em 0;
  }
</style>
