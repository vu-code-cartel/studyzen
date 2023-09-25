import { useTranslation } from 'react-i18next';
import { useDocumentTitle } from '@mantine/hooks';
import { Stack, Title, Text, Button } from '@mantine/core';
import { AppRoutes } from '../common/app-routes';
import { Link } from 'react-router-dom';
import { useButtonVariant } from '../hooks/useButtonVariant';

export const NotFound = () => {
  const { t } = useTranslation();
  const buttonVariant = useButtonVariant();
  useDocumentTitle(t('NotFound.DocumentTitle'));

  return (
    <Stack align='center' gap='sm' pt='30vh' p='lg'>
      <Title>{t('NotFound.404')}</Title>
      <Text fw={600} ta='center'>
        {t('NotFound.Title')}
      </Text>
      <Button component={Link} to={AppRoutes.Home} color='teal' variant={buttonVariant}>
        {t('NotFound.GoBackHome')}
      </Button>
    </Stack>
  );
};
