import { Text } from '@mantine/core';
import { useTranslation } from 'react-i18next';
import { PageHeader } from '../components/PageHeader';
import { useDocumentTitle } from '@mantine/hooks';
import { usePageCategory } from '../hooks/usePageCategory';

export const HomePage = () => {
  const { t } = useTranslation();

  useDocumentTitle(t('HomePage.DocumentTitle'));
  usePageCategory('unknown');

  return (
    <PageHeader>
      <Text fw={600}>{t('HomePage.Title')}</Text>
    </PageHeader>
  );
};
