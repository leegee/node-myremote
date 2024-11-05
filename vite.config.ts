import { defineConfig } from 'vite';
import { svelte } from '@sveltejs/vite-plugin-svelte';
import { viteSingleFile } from 'vite-plugin-singlefile';

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    svelte(),
    viteSingleFile(),
  ],
  define: {
    'process.env': {
      VITE_WS_PORT: process.env.VITE_WS_PORT || 8223,
      VITE_APP_TITLE: process.env.VITE_APP_TITLE || "MyRemote",
    },
  },
  build: {
    target: 'esnext',
    assetsInlineLimit: 100000000,
    outDir: 'dist',
  },
});
