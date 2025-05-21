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
  <aside class="mask">
    <dialog open class="modal">
      <header>
        <h2>
          <button on:click={closeModal} aria-label="Back" title="Back"
            >﹤</button
          >
          {modalTitle}
        </h2>
      </header>
      <div class="modal-content">
        <slot></slot>
      </div>
    </dialog>
  </aside>
{/if}

<style>
  .mask {
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    background-color: rgba(0, 0, 0, 0.7);
    display: flex;
    justify-content: center;
    align-items: center;
  }

  .modal {
    padding: 2em;
    border: none;
    border-radius: 0.5em;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    width: auto;
    max-width: 45em;
    max-height: 100%;
    overflow: hidden;
  }

  header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    position: relative;
    left: -1.5em;
  }

  h2 {
    font-size: 1.2em;
    margin: 0;
    padding: 0;
  }

  button[aria-label="Back"] {
    background: none;
    border: none;
    padding: 0;
    margin: 0;
    margin-right: 0.5em;
    cursor: pointer;
    font-weight: bolder;
  }

  .modal-content {
    padding: 1em 0;
    overflow: auto;
    height: 100vh;
  }
</style>
