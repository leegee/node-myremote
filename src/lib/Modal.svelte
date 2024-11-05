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
      <h2>{modalTitle}</h2>
      <button on:click={closeModal} aria-label="Close">✖</button>
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
    width: 100vw;
    height: 100vh;
    border: none;
    border-radius: 8px;
    padding: 20px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
  }

  header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-bottom: 1px solid #ccc;
    padding-bottom: 10px;
    margin-bottom: 20px;
  }

  h2 {
    font-size: 1.2em;
    margin: 0;
  }

  button[aria-label="Close"] {
    background: none;
    border: none;
    font-size: 1.2em;
    cursor: pointer;
  }

  .modal-content {
    padding: 10px 0;
  }
</style>
