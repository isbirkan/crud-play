import { dirname } from 'path';
import { fileURLToPath } from 'url';

import { FlatCompat } from '@eslint/eslintrc';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

const compat = new FlatCompat({
  baseDirectory: __dirname
});

const eslintConfig = async () => {
  const importPlugin = (await import('eslint-plugin-import')).default;
  const jsxA11yPlugin = (await import('eslint-plugin-jsx-a11y')).default;
  const prettierPlugin = (await import('eslint-plugin-prettier')).default;
  const reactPlugin = (await import('eslint-plugin-react')).default;

  return [
    ...compat.extends(
      'next/core-web-vitals',
      'next/typescript',
      'prettier',
      'plugin:react/recommended',
      'plugin:jsx-a11y/recommended',
      'plugin:import/errors',
      'plugin:import/warnings'
    ),
    {
      plugins: {
        import: importPlugin,
        'jsx-a11y': jsxA11yPlugin,
        prettier: prettierPlugin,
        react: reactPlugin
      },
      rules: {
        '@typescript-eslint/no-explicit-any': 'off',
        'comma-dangle': ['error', 'never'],
        'import/no-anonymous-default-export': 'off',
        'import/no-unresolved': 'error',
        'import/order': [
          'error',
          {
            'newlines-between': 'always',
            pathGroups: [
              {
                pattern: '@/**',
                group: 'unknown',
                position: 'after'
              }
            ],
            groups: ['builtin', 'external', 'parent', 'sibling', 'index', 'unknown'],
            warnOnUnassignedImports: true,
            pathGroupsExcludedImportTypes: ['builtin'],
            alphabetize: { order: 'asc', caseInsensitive: true },
            distinctGroup: true,
            named: false
          }
        ],
        'jsx-a11y/label-has-associated-control': 'off',
        'prettier/prettier': [
          'error',
          {
            bracketSameLine: true,
            endOfLine: 'auto',
            printWidth: 120,
            singleQuote: true,
            tabWidth: 2,
            trailingComma: 'none'
          }
        ],
        'react/react-in-jsx-scope': 'off'

        // "func-names": "off",
        // "import/default": "off",
        // "import/no-extraneous-dependencies": "warn",
        // "import/prefer-default-export": "off",
        // "jsx-a11y/anchor-is-valid": "off",
        // "jsx-a11y/label-has-for": "off",
        // "linebreak-style": "off",
        // "no-console": "off",
        // "no-nested-ternary": "warn",
        // "no-param-reassign": "warn",
        // "no-shadow": "warn",
        // "no-unused-vars": "warn",
        // "prefer-destructuring": "off",
        // "react/destructuring-assignment": "off",
        // "react/display-name": "off",
        // "react/jsx-filename-extension": ["warn", { extensions: [".js", ".jsx"] }],
        // "react/no-access-state-in-setstate": "off",
        // "react/no-array-index-key": "off",
        // "react/prop-types": ["warn", { skipUndeclared: true }],
      }
    }
  ];
};

export default await eslintConfig();
