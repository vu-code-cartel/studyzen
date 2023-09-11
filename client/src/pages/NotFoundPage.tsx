import { Text } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';

export const NotFoundPage = () => {
  const { t } = useTranslation();

  return <Text>{t('NotFoundPage')}</Text>;
};
