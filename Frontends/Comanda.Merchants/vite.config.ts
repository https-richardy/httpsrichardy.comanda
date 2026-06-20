///<reference types="vitest" />

import tailwindcss from '@tailwindcss/vite';
import react from '@vitejs/plugin-react-swc';
import path from 'path';
import { defineConfig } from 'vitest/config';

export default defineConfig({
  plugins: [react(), tailwindcss()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, 'src')
    }
  },
  server: {
    proxy: {
      // Proxy para a API oficial da Huggy em desenvolvimento, evitando CORS.
      // O client (infra/api/huggy-api) usa este caminho quando em DEV.
      '/huggy-api': {
        target: 'https://api.huggy.app',
        changeOrigin: true,
        secure: true,
        rewrite: (requestPath) => requestPath.replace(/^\/huggy-api/, '')
      }
    }
  },
  test: {
    globals: true,
    environment: 'jsdom',
    setupFiles: ['./src/test/setup.ts', 'vitest-localstorage-mock'],
    css: true
  }
});
