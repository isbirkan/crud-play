export const commonRules = {
  '@typescript-eslint/no-explicit-any': 'off',
  '@typescript-eslint/no-unused-vars': ['warn', { argsIgnorePattern: '^_' }],
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
          group: 'internal',
          position: 'after'
        }
      ],
      groups: ['builtin', 'external', 'parent', 'sibling', 'index', 'internal'],
      warnOnUnassignedImports: true,
      pathGroupsExcludedImportTypes: ['builtin'],
      alphabetize: { order: 'asc', caseInsensitive: true },
      distinctGroup: true,
      named: false
    }
  ],
  'jsx-a11y/label-has-associated-control': 'off',
  'react/react-in-jsx-scope': 'off',
  'react-refresh/only-export-components': ['warn', { allowConstantExport: true }]

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
  // "react/prop-types": ["warn", { skipUndeclared: true }]
};
