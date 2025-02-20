import { commonRules } from './eslint-common.mjs';
import { importPlugins } from './eslint-plugins.mjs';

const config = async () => {
  const plugins = await importPlugins();

  return {
    files: ['**/*.{js,jsx,mjs,cjs}'],
    ignores: ['dist', 'node_modules'],
    languageOptions: {
      ecmaVersion: 'latest',
      sourceType: 'module'
    },
    settings: {
      react: {
        version: 'detect'
      }
    },
    plugins,
    rules: {
      ...commonRules
    }
  };
};

export default config;