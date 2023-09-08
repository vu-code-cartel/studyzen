import { Text } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';

const MainPage = () => {
  const { t } = useTranslation();

  return <Text>{t('MainPage')}</Text>;
};

export default MainPage;
