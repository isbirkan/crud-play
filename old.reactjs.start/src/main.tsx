import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';

import App from './App.tsx';
import './global.scss';

if (import.meta.env.NODE_ENV === 'production') {
  console.log = function () {};
  console.info = function () {};
}

const target = document.getElementById('root')!;
const template = (
  <StrictMode>
    <App />
  </StrictMode>
);

createRoot(target).render(template);