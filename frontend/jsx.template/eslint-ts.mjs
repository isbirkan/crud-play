import { commonRules } from './eslint-common.mjs';
import { importPlugins, importParser } from './eslint-plugins.mjs';

const config = async () => {
  const plugins = await importPlugins();
  const parser = await importParser(); 

  return {
    files: ['**/*.{ts,tsx}'],
    ignores: ['dist', 'node_modules'],
    languageOptions: {
      ecmaVersion: 'latest',
      sourceType: 'module',
      parser,
      parserOptions: {
        project: './tsconfig.eslint.json'
      }
    },
    settings: {
      react: {
        version: 'detect'
      },
      'import/parsers': { '@typescript-eslint/parser': ['.ts', '.tsx'] },
      'import/resolver': {
        typescript: {
          project: './tsconfig.app.json',
          alwaysTryTypes: true
        }
      },
      'tailwindcss': {
        whitelist: ['@tailwind', '@apply', '@variants', '@responsive']
      }
    },
    plugins,
    rules: {
      ...commonRules,
      'tailwindcss/classnames-order': 'warn',
      '@typescript-eslint/no-explicit-any': 'off',
      '@typescript-eslint/no-unused-vars': ['warn', { argsIgnorePattern: '^_', varsIgnorePattern: '^_' }]
    }
  };
};

export default config;