import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';

import App from './app/app';
import { bootstrapApp } from './app/bootstrap';

import './index.css';

bootstrapApp();

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>
);
