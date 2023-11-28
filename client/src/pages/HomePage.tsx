import { Text } from '@mantine/core';
import { useTranslation } from 'react-i18next';
import { PageHeader } from '../components/PageHeader';
import { useDocumentTitle } from '@mantine/hooks';
import { usePageCategory } from '../hooks/usePageCategory';
import { PageContainer } from '../components/PageContainer';
import { useAppStore } from '../hooks/useAppStore';

export const HomePage = () => {
  const { t } = useTranslation();
  useDocumentTitle(t('Home.DocumentTitle'));
  usePageCategory('unknown');

  const isLoggedIn = useAppStore((state) => state.user);
  const username = useAppStore((state) => state.user?.username);

  return (
    <PageContainer>
      <PageHeader>
        {isLoggedIn ? (
          <Text fw={600}>{t('Home.WelcomeMessage', { username: username })}</Text>
        ) : (
          <Text fw={600}>{t('Home.Title')}</Text>
        )}
      </PageHeader>
    </PageContainer>
  );
};
