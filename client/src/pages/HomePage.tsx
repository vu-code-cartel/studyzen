import { Text } from '@mantine/core';
import { useTranslation } from 'react-i18next';
import { PageHeader } from '../components/PageHeader';
import { useDocumentTitle } from '@mantine/hooks';
import { usePageCategory } from '../hooks/usePageCategory';
import { PageContainer } from '../components/PageContainer';

export const HomePage = () => {
  const { t } = useTranslation();
  useDocumentTitle(t('Home.DocumentTitle'));
  usePageCategory('unknown');

  return (
    <PageContainer>
      <PageHeader>
        <Text fw={600}>{t('Home.Title')}</Text>
      </PageHeader>
    </PageContainer>
  );
};
