import { defineConfig } from 'vite';
import { svelte } from '@sveltejs/vite-plugin-svelte';

// https://vite.dev/config/
export default defineConfig({
  plugins: [svelte()],
  define: {
    'process.env': {
      VITE_WS_PORT: process.env.VITE_WS_PORT || 8223,
      VITE_APP_TITLE: process.env.VITE_APP_TITLE || "MyRemote",
    },
  },
});
