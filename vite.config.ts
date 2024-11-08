import { defineConfig } from 'vite';
import { svelte } from '@sveltejs/vite-plugin-svelte';
import { viteSingleFile } from 'vite-plugin-singlefile';
import dotenv from 'dotenv';

dotenv.config();

// Define required environment variables
const requiredEnvVars = ['VITE_WS_PORT', 'VITE_APP_TITLE', 'VITE_APP_RE'] as const;

const missingEnvVars = requiredEnvVars.filter(varName => !process.env[varName]);

if (missingEnvVars.length > 0) {
  console.error(`Missing required environment variables: ${missingEnvVars.join(', ')}`);
  process.exit(1);
}

const processEnv: Record<string, string | undefined> = requiredEnvVars.reduce((acc, varName) => {
  acc[varName] = process.env[varName];
  return acc;
}, {} as Record<string, string | undefined>);

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    svelte(),
    viteSingleFile(),
  ],
  define: {
    'process.env': processEnv,
  },
  build: {
    target: 'esnext',
    assetsInlineLimit: 100000000,
    outDir: 'dist'
  },
});
