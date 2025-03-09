import type { Config } from 'tailwindcss';

export default {
  content: ['./src/**/*.{html,ts}'],
  theme: {
    extend: {
      colors: {},
      fontFamily: {
        roboto: ['Roboto', 'sans-serif'],
        bebas: ['"Bebas Neue"', 'cursive'],
        dancing: ['"Dancing Script"', 'sans-serif'],
        aller: ['Aller', 'sans-serif']
      }
    }
  },
  plugins: []
} satisfies Config;
