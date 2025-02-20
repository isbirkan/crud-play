import { dirname } from 'path';
import { fileURLToPath } from 'url';

import { FlatCompat } from '@eslint/eslintrc';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

const compat = new FlatCompat({
  baseDirectory: __dirname,
  recommendedConfig: { plugins: [], rules: {} }
});

import jsConfig from './eslint-js.mjs';
import tsConfig from './eslint-ts.mjs';

const eslintConfig = async () => {
  return [
    ...compat.extends(
      'eslint:recommended',
      'plugin:import/recommended',
      'plugin:jsx-a11y/recommended',
      'plugin:react/recommended',
      'plugin:tailwindcss/recommended',
      'prettier' // should be last
    ),
    await jsConfig(),
    await tsConfig()
  ];
};

export default await eslintConfig();