import { defineConfig } from 'vite';
import { svelte } from '@sveltejs/vite-plugin-svelte';
import { viteSingleFile } from 'vite-plugin-singlefile';
import dotenv from 'dotenv';
dotenv.config();
// Define required environment variables
var requiredEnvVars = ['VITE_WS_PORT', 'VITE_APP_TITLE', 'VITE_APP_RE'];
var missingEnvVars = requiredEnvVars.filter(function (varName) { return !process.env[varName]; });
if (missingEnvVars.length > 0) {
    console.error("Missing required environment variables: ".concat(missingEnvVars.join(', ')));
    process.exit(1);
}
var processEnv = requiredEnvVars.reduce(function (acc, varName) {
    acc[varName] = process.env[varName]; // Assign the environment variable
    return acc;
}, {});
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
