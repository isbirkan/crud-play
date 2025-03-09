// @ts-check
import angularPlugin from '@angular-eslint/eslint-plugin';
import angularTemplateParser from '@angular-eslint/template-parser';
import eslintPluginPrettierRecommended from 'eslint-plugin-prettier/recommended';
import * as importPlugin from 'eslint-plugin-import';
import prettierPlugin from 'eslint-plugin-prettier';
import tailwindcssPlugin from 'eslint-plugin-tailwindcss';
import tsPlugin from '@typescript-eslint/eslint-plugin';
import typescriptParser from '@typescript-eslint/parser';

export default [
  {
    ignores: ['.cache/', '.git/', '.github/', 'node_modules/']
  },
  {
    files: ['**/*.ts'],
    settings: {
      'import/resolver': {
        typescript: {
          project: './tsconfig.json',
          alwaysTryTypes: true
        },
        node: {
          extensions: ['.js', '.ts']
        }
      }
    },
    languageOptions: {
      parser: typescriptParser,
      parserOptions: {
        project: ['./tsconfig.json', './tsconfig.app.json', './tsconfig.spec.json']
      }
    },
    plugins: {
      '@typescript-eslint': tsPlugin,
      '@angular-eslint': angularPlugin,
      prettier: prettierPlugin,
      import: importPlugin.default,
      tailwindcss: tailwindcssPlugin
    },
    rules: {
      ...tsPlugin.configs['recommended'].rules,
      ...tsPlugin.configs['stylistic'].rules,
      ...angularPlugin.configs.recommended.rules,
      ...(prettierPlugin.configs ? prettierPlugin.configs['rules'] : {}),

      '@angular-eslint/directive-selector': [
        'error',
        {
          type: 'attribute',
          prefix: 'app',
          style: 'camelCase'
        }
      ],
      '@angular-eslint/component-selector': [
        'error',
        {
          type: 'element',
          prefix: 'app',
          style: 'kebab-case'
        }
      ],
      '@typescript-eslint/no-explicit-any': 'off',
      //'@typescript-eslint/quotes': ['error', 'single'],
      'comma-dangle': ['error', 'never'],
      'import/order': [
        'error',
        {
          'newlines-between': 'always',
          groups: ['builtin', 'external', 'internal', 'parent', 'sibling', 'index'],
          pathGroups: [
            {
              pattern: '@/**',
              group: 'internal',
              position: 'before'
            }
          ],
          pathGroupsExcludedImportTypes: ['builtin'],
          alphabetize: { order: 'asc', caseInsensitive: true }
        }
      ]
    }
  },
  {
    files: ['**/*.html'],
    languageOptions: {
      parser: angularTemplateParser
    },
    plugins: {
      '@angular-eslint': angularPlugin,
      '@angular-eslint/template': angularPlugin,
      prettier: prettierPlugin
    },
    rules: {
      'prettier/prettier': ['error', { parser: 'angular' }]
    }
  },
  eslintPluginPrettierRecommended
];
