import { Text } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';

const NotFoundPage = () => {
  const { t } = useTranslation();

  return <Text>{t('NotFoundPage')}</Text>;
};

export default NotFoundPage;
