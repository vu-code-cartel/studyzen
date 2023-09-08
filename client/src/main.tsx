import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import Providers from './providers/Providers.tsx';
import './i18n/i18n.ts';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <Providers />
  </React.StrictMode>,
);
