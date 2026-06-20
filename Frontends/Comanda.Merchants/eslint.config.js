import js from '@eslint/js';
import globals from 'globals';
import reactHooks from 'eslint-plugin-react-hooks';
import reactRefresh from 'eslint-plugin-react-refresh';
import tseslint from 'typescript-eslint';
import prettierRecommended from 'eslint-plugin-prettier/recommended';
import boundaries from 'eslint-plugin-boundaries';
import simpleImportSort from 'eslint-plugin-simple-import-sort';
import eslintConfigPrettier from 'eslint-config-prettier';
import { defineConfig, globalIgnores } from 'eslint/config';

const PUBLIC_FEATURE_API_MESSAGE =
  '❌ Deep import entre features é proibido. Use apenas a API pública da feature: "@/features/nome-da-feature". Dentro da própria feature, use imports relativos ou "@/features/sua-feature/...".';

// ---------------------------------------------------------------------------
// Regra customizada: permite alias absoluto intra-feature (@/features/X/...)
// mas bloqueia deep imports cross-feature — inclusive em dynamic imports.
// ---------------------------------------------------------------------------
function checkDeepFeatureImport(context, importPath, reportNode) {
  // Só nos importa caminhos que entram em sub-pastas de uma feature
  const importFeatureMatch = importPath.match(/^@\/features\/([^/]+)\/.+/);
  if (!importFeatureMatch) return;

  const importedFeature = importFeatureMatch[1];

  // Descobre a feature do arquivo sendo analisado
  const filename = context.filename ?? context.getFilename?.();
  const currentFeatureMatch = filename.match(
    /[/\\]features[/\\]([^/\\]+)[/\\]/
  );

  // Se o arquivo atual não pertence a nenhuma feature OU pertence a uma
  // feature diferente da importada → deep import cross-feature → erro
  if (!currentFeatureMatch || currentFeatureMatch[1] !== importedFeature) {
    context.report({
      node: reportNode,
      message: PUBLIC_FEATURE_API_MESSAGE
    });
  }
  // Mesma feature → import absoluto intra-feature → permitido (sem report)
}

const featureDeepImportRule = {
  meta: {
    type: 'problem',
    docs: {
      description:
        'Proíbe deep imports cross-feature via alias absoluto, mas permite intra-feature.'
    },
    schema: []
  },
  create(context) {
    return {
      // import estático: import { X } from '@/features/...'
      ImportDeclaration(node) {
        checkDeepFeatureImport(context, node.source.value, node);
      },
      // dynamic import: import('@/features/...')  ← o caso que quebrava
      ImportExpression(node) {
        if (
          node.source.type === 'Literal' &&
          typeof node.source.value === 'string'
        ) {
          checkDeepFeatureImport(context, node.source.value, node);
        }
      }
    };
  }
};

export default defineConfig([
  globalIgnores([
    'dist',
    'build',
    'coverage',
    'node_modules',
    '.turbo',
    '.next',
    'storybook-static'
  ]),
  {
    files: ['**/*.{ts,tsx}'],

    extends: [
      js.configs.recommended,
      ...tseslint.configs.recommended,
      reactHooks.configs['recommended-latest'],
      reactRefresh.configs.vite
    ],

    languageOptions: {
      ecmaVersion: 2022,
      sourceType: 'module',
      globals: globals.browser
    },

    plugins: {
      boundaries,
      'simple-import-sort': simpleImportSort,
      // Plugin inline com a regra customizada
      'local-rules': {
        rules: {
          'feature-deep-import': featureDeepImportRule
        }
      }
    },
    settings: {
      'boundaries/include': ['src/**/*'],
      'boundaries/elements': [
        {
          type: 'app',
          pattern: 'src/app/**/*'
        },
        {
          type: 'shared',
          pattern: 'src/shared/**/*'
        },
        {
          type: 'infra',
          pattern: 'src/infra/**/*'
        },
        {
          type: 'feature-public-api',
          pattern: 'src/features/*/index.ts',
          mode: 'file'
        },
        {
          type: 'feature-presentation',
          pattern: 'src/features/*/presentation/**/*'
        },
        {
          type: 'feature-domain-services',
          pattern: 'src/features/*/domain/services/**/*'
        },
        {
          type: 'feature-domain-entities',
          pattern: 'src/features/*/domain/entities/**/*'
        },
        {
          type: 'feature-domain-consts',
          pattern: 'src/features/*/domain/consts/**/*'
        },
        {
          type: 'feature-domain-utils',
          pattern: 'src/features/*/domain/utils/**/*'
        },
        {
          type: 'feature-domain-private',
          pattern: 'src/features/*/domain/**/*'
        },
        {
          type: 'feature-data-api',
          pattern: 'src/features/*/data/api/**/*'
        },
        {
          type: 'feature-data-private',
          pattern: 'src/features/*/data/**/*'
        },
        {
          type: 'feature-private',
          pattern: 'src/features/*/**/*'
        }
      ]
    },
    rules: {
      'react-refresh/only-export-components': 'off',
      '@typescript-eslint/no-explicit-any': 'warn',
      'prettier/prettier': [
        'error',
        {
          endOfLine: 'auto'
        }
      ],
      '@typescript-eslint/consistent-type-imports': [
        'error',
        {
          prefer: 'type-imports',
          fixStyle: 'inline-type-imports'
        }
      ],
      '@typescript-eslint/no-unused-vars': [
        'error',
        {
          argsIgnorePattern: '^_',
          varsIgnorePattern: '^_',
          caughtErrorsIgnorePattern: '^_'
        }
      ],
      'simple-import-sort/exports': 'error',
      'simple-import-sort/imports': [
        'error',
        {
          groups: [
            ['^react$', '^react-dom', '^react-router', '^@?\\w'],
            ['^@/app(/.*|$)'],
            ['^@/features(/.*|$)'],
            ['^@/shared(/.*|$)'],
            ['^@/infra(/.*|$)'],
            ['^\\.\\.(?!/?$)', '^\\.\\./?$'],
            ['^\\./(?=.*/)(?!/?$)', '^\\.(?!/?$)', '^\\./?$'],
            ['^.+\\.s?css$']
          ]
        }
      ],
      // Substituída pela regra customizada 'local-rules/feature-deep-import'
      // que distingue intra-feature (OK) de cross-feature (erro) e também
      // cobre dynamic imports (import('...')).
      'local-rules/feature-deep-import': 'error',
      'boundaries/dependencies': [
        'error',
        {
          default: 'disallow',
          rules: [
            {
              from: ['app'],
              allow: ['app', 'shared', 'feature-public-api', 'infra']
            },
            {
              from: ['infra'],
              allow: ['infra', 'shared']
            },
            {
              from: ['shared'],
              allow: ['shared']
            },
            {
              from: ['feature-public-api'],
              allow: [
                'feature-presentation',
                'feature-domain-services',
                'feature-domain-entities',
                'feature-domain-consts',
                'feature-domain-utils',
                'feature-data-api'
              ]
            },
            {
              from: ['feature-presentation'],
              allow: [
                'feature-presentation',
                'feature-domain-services',
                'feature-domain-entities',
                'feature-domain-consts',
                'feature-domain-utils',
                'feature-data-api',
                'feature-public-api',
                'shared'
              ]
            },
            {
              from: ['feature-domain-services'],
              allow: [
                'feature-domain-services',
                'feature-domain-entities',
                'feature-domain-consts',
                'feature-domain-utils',
                'feature-data-api',
                'feature-public-api',
                'shared'
              ]
            },
            {
              from: ['feature-domain-entities'],
              allow: [
                'feature-domain-entities',
                'feature-domain-consts',
                'feature-domain-utils',
                'feature-public-api',
                'shared'
              ]
            },
            {
              from: ['feature-domain-consts'],
              allow: ['feature-domain-consts', 'shared']
            },
            {
              from: ['feature-domain-utils'],
              allow: [
                'feature-domain-utils',
                'feature-domain-entities',
                'feature-domain-consts',
                'shared'
              ]
            },
            {
              from: ['feature-domain-private'],
              allow: [
                'feature-domain-services',
                'feature-domain-entities',
                'feature-domain-consts',
                'feature-domain-utils',
                'feature-domain-private',
                'shared'
              ]
            },
            {
              from: ['feature-domain-private'],
              allow: [
                'feature-domain-services',
                'feature-domain-entities',
                'feature-domain-consts',
                'feature-domain-utils',
                'feature-domain-private',
                'shared'
              ]
            },
            {
              from: ['feature-data-api'],
              allow: [
                'feature-data-api',
                'feature-data-private',

                'feature-domain-entities',
                'feature-domain-consts',
                'feature-domain-utils',

                'infra',
                'shared'
              ]
            },
            {
              from: ['feature-data-private'],
              allow: [
                'feature-data-api',
                'feature-data-private',
                'feature-domain-entities',
                'feature-domain-consts',
                'feature-domain-utils',
                'infra',
                'shared'
              ]
            },
            {
              from: ['feature-private'],
              allow: [
                'feature-public-api',

                'feature-domain-services',
                'feature-domain-entities',
                'feature-domain-consts',
                'feature-domain-utils',

                'feature-data-api',

                'shared'
              ]
            }
          ]
        }
      ]
    }
  },
  eslintConfigPrettier,
  prettierRecommended
]);
