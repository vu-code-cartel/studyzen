import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import './i18n/i18n.ts';
import { Providers } from './providers/Providers.tsx';
import '@mantine/core/styles.css';
import '@mantine/tiptap/styles.css';
import '@mantine/notifications/styles.css';
import tsLanguageSyntax from 'highlight.js/lib/languages/typescript';
import csharpLanguageSyntax from 'highlight.js/lib/languages/csharp';
import { createLowlight } from 'lowlight';

export const lowlight = createLowlight();
lowlight.register('ts', tsLanguageSyntax);
lowlight.register('csharp', csharpLanguageSyntax);

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <Providers />
  </React.StrictMode>,
);
