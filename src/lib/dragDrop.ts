import type { Writable } from "svelte/store";
import type { Command } from "../types/commands";

export function setupDragAndDrop(items: any[], commandsStore: Writable<Command[]>) {
    let draggedIndex: number | null = null;

    function handleDragStart(index: number) {
        draggedIndex = index;
    }

    function handleDragOver(event: DragEvent) {
        event.preventDefault(); // Required to allow dropping

    }

    function handleDragEnd() {
        draggedIndex = null;
        document.body.style.cursor = "default";
    }

    function handleDrop(targetIndex: number, event: Event) {
        event.preventDefault();
        if (draggedIndex === null || draggedIndex === targetIndex) return;

        const updatedItems = [...items];
        const draggedItem = updatedItems[draggedIndex];
        updatedItems.splice(draggedIndex, 1);
        updatedItems.splice(targetIndex, 0, draggedItem);

        commandsStore.set(updatedItems);
        draggedIndex = null;
    }

    return { handleDragStart, handleDragOver, handleDragEnd, handleDrop };
}
