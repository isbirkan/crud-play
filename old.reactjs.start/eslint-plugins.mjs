
/* eslint-disable import/no-unresolved */

const importPlugins = async () => {
  return {
    import: (await import('eslint-plugin-import')).default,
    'jsx-a11y': (await import('eslint-plugin-jsx-a11y')).default,
    react: (await import('eslint-plugin-react')).default,
    'react-hooks': (await import('eslint-plugin-react-hooks')).default,
    'react-refresh': (await import('eslint-plugin-react-refresh')).default,
    tailwindcss: (await import('eslint-plugin-tailwindcss')).default,
    '@typescript-eslint': (
      (await import('@typescript-eslint/eslint-plugin')).default ||
      (await import('@typescript-eslint/eslint-plugin'))
    )
  };
};

const importParser = async () => {
  return (
    (await import('@typescript-eslint/parser')).default ||
    (await import('@typescript-eslint/parser'))
  );
};

export { importPlugins, importParser };