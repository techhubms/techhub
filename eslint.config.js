export default [
    // Ignore third-party libraries
    {
        ignores: [
            'src/techhub.webapp/techhub.webapp.Web/wwwroot/lib/**/*',
            'node_modules/**/*'
        ]
    },
    // Browser/Client-side JavaScript files
    {
        files: ["assets/js/**/*.js"],
        languageOptions: {
            ecmaVersion: 2022,
            sourceType: "script",
            globals: {
                // Browser globals
                window: "readonly",
                document: "readonly",
                console: "readonly",
                
                // Common browser APIs
                fetch: "readonly",
                URL: "readonly",
                URLSearchParams: "readonly",
                Set: "readonly",
                Map: "readonly",
                Array: "readonly",
                Object: "readonly",
                RegExp: "readonly",
                parseInt: "readonly",
                Math: "readonly",
                Date: "readonly",
                setTimeout: "readonly",
                clearTimeout: "readonly",
                setInterval: "readonly",
                clearInterval: "readonly",
                
                // Node.js CommonJS for browser builds
                module: "readonly",
                exports: "readonly"
            }
        },
        rules: {
            // Error prevention
            "no-unused-vars": ["error", {
                "vars": "all",
                "args": "after-used",
                "ignoreRestSiblings": false,
                "argsIgnorePattern": "^_"
            }],
            "no-undef": "error",
            "no-redeclare": "error",
            "no-dupe-keys": "error",
            "no-duplicate-case": "error",
            "no-unreachable": "error",

            // Code quality
            "no-var": "error",
            "prefer-const": "error",
            "no-eval": "error",
            "no-implied-eval": "error",
            "no-new-func": "error",

            // Style consistency
            "indent": ["error", 4],
            "quotes": ["error", "single", { "avoidEscape": true }],
            "semi": ["error", "always"],
            "comma-dangle": ["error", "never"],
            "no-trailing-spaces": "error",
            "eol-last": "error",

            // Best practices
            "eqeqeq": ["error", "always"],
            // Disable for browser scripts that need global functions
            "no-implicit-globals": "off",
            "no-global-assign": "error",
            "no-extend-native": "error",

            // Warnings for potential issues
            "no-console": "warn",
            "no-debugger": "warn"
        }
    },
    // Playwright E2E test files (run in browser context)
    {
        files: ["spec/e2e/tests/**/*.js", "spec/e2e/tests/*.js"],
        languageOptions: {
            ecmaVersion: 2022,
            sourceType: "script",
            globals: {
                // Node.js globals for test setup
                require: "readonly",
                module: "readonly",
                exports: "readonly",
                process: "readonly",
                __dirname: "readonly",
                __filename: "readonly",
                console: "readonly",
                
                // Browser globals (tests run in browser context)
                window: "readonly",
                document: "readonly",
                fetch: "readonly",
                URL: "readonly",
                URLSearchParams: "readonly",
                performance: "readonly",
                
                // Common globals
                setTimeout: "readonly",
                clearTimeout: "readonly",
                setInterval: "readonly",
                clearInterval: "readonly"
            }
        },
        rules: {
            // Error prevention
            "no-unused-vars": ["error", {
                "vars": "all",
                "args": "after-used",
                "ignoreRestSiblings": false,
                "argsIgnorePattern": "^_"
            }],
            "no-undef": "error",
            "no-redeclare": "error",
            "no-dupe-keys": "error",
            "no-duplicate-case": "error",
            "no-unreachable": "error",

            // Code quality
            "no-var": "error",
            "prefer-const": "error",
            "no-eval": "error",
            "no-implied-eval": "error",
            "no-new-func": "error",

            // Style consistency - Use 2 spaces for e2e files to match existing style
            "indent": ["error", 2],
            "quotes": ["error", "single", { "avoidEscape": true }],
            "semi": ["error", "always"],
            "comma-dangle": ["error", "never"],
            "no-trailing-spaces": "error",
            "eol-last": "error",

            // Best practices
            "eqeqeq": ["error", "always"],
            "no-global-assign": "error",
            "no-extend-native": "error",

            // Allow console in tests
            "no-console": "off",
            "no-debugger": "warn"
        }
    },
    // Other Node.js script files (non-test)
    {
        files: ["spec/e2e/**/*.js"],
        ignores: ["spec/e2e/tests/**/*.js", "spec/e2e/tests/*.js"],
        languageOptions: {
            ecmaVersion: 2022,
            sourceType: "script",
            globals: {
                // Node.js globals
                require: "readonly",
                module: "readonly",
                exports: "readonly",
                process: "readonly",
                __dirname: "readonly",
                __filename: "readonly",
                global: "readonly",
                console: "readonly",
                Buffer: "readonly",
                
                // Common globals
                setTimeout: "readonly",
                clearTimeout: "readonly",
                setInterval: "readonly",
                clearInterval: "readonly"
            }
        },
        rules: {
            // Error prevention
            "no-unused-vars": ["error", {
                "vars": "all",
                "args": "after-used",
                "ignoreRestSiblings": false,
                "argsIgnorePattern": "^_"
            }],
            "no-undef": "error",
            "no-redeclare": "error",
            "no-dupe-keys": "error",
            "no-duplicate-case": "error",
            "no-unreachable": "error",

            // Code quality
            "no-var": "error",
            "prefer-const": "error",
            "no-eval": "error",
            "no-implied-eval": "error",
            "no-new-func": "error",

            // Style consistency - Use 2 spaces for e2e files to match existing style
            "indent": ["error", 2],
            "quotes": ["error", "single", { "avoidEscape": true }],
            "semi": ["error", "always"],
            "comma-dangle": ["error", "never"],
            "no-trailing-spaces": "error",
            "eol-last": "error",

            // Best practices
            "eqeqeq": ["error", "always"],
            "no-global-assign": "error",
            "no-extend-native": "error",

            // Allow console in Node.js scripts
            "no-console": "off",
            "no-debugger": "warn"
        }
    },
    // Jest test files
    {
        files: ["spec/javascript/**/*.js"],
        languageOptions: {
            ecmaVersion: 2022,
            sourceType: "script",
            globals: {
                // Jest/Testing globals
                describe: "readonly",
                test: "readonly",
                it: "readonly",
                expect: "readonly",
                beforeEach: "readonly",
                afterEach: "readonly",
                beforeAll: "readonly",
                afterAll: "readonly",
                jest: "readonly",
                
                // Node.js globals for test environment
                require: "readonly",
                module: "readonly",
                exports: "readonly",
                process: "readonly",
                __dirname: "readonly",
                __filename: "readonly",
                global: "readonly",
                console: "readonly",
                
                // Browser globals for testing browser code
                window: "readonly",
                document: "readonly",
                fetch: "readonly",
                URL: "readonly",
                URLSearchParams: "readonly",
                performance: "readonly",
                
                // Test helper functions (defined in test-setup.js)
                createMockElement: "readonly",
                createMockFilterData: "readonly",
                createMockTagRelationships: "readonly",
                createMockDateConfig: "readonly"
            }
        },
        rules: {
            // Error prevention
            "no-unused-vars": ["error", {
                "vars": "all",
                "args": "after-used",
                "ignoreRestSiblings": false,
                "argsIgnorePattern": "^_"
            }],
            "no-undef": "error",
            "no-redeclare": "error",
            "no-dupe-keys": "error",
            "no-duplicate-case": "error",
            "no-unreachable": "error",

            // Code quality
            "no-var": "error",
            "prefer-const": "error",
            "no-eval": "error",
            "no-implied-eval": "error",
            "no-new-func": "error",

            // Style consistency - Use 2 spaces for test files to match existing style
            "indent": ["error", 2],
            "quotes": ["error", "single", { "avoidEscape": true }],
            "semi": ["error", "always"],
            "comma-dangle": ["error", "never"],
            "no-trailing-spaces": "error",
            "eol-last": "error",

            // Best practices
            "eqeqeq": ["error", "always"],
            "no-global-assign": "error",
            "no-extend-native": "error",

            // Allow console in tests
            "no-console": "off",
            "no-debugger": "warn"
        }
    }
];
