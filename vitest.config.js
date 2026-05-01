import { defineConfig } from 'vitest/config';

export default defineConfig({
    test: {
        environment: 'jsdom',
        root: '.',
        include: ['tests/javascript/**/*.test.js'],
    },
});
