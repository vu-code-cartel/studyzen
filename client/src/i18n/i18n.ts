import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import en from './en.json';

export const resources = {
  en: {
    translation: en,
  },
};

i18n.use(initReactI18next).init({
  debug: true,
  resources: resources,
  lng: 'en',
  fallbackLng: 'en',
  keySeparator: '.',
  interpolation: {
    escapeValue: false,
  },
});

export default i18n;