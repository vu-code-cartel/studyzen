import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import './i18n/i18n.ts';
import { Providers } from './providers/Providers.tsx';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <Providers />
  </React.StrictMode>,
);
