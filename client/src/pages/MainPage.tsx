import { Text } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';

export const MainPage = () => {
  const { t } = useTranslation();

  return <Text>{t('MainPage')}</Text>;
};
