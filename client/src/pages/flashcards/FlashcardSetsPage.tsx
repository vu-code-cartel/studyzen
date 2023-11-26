import { useTranslation } from 'react-i18next';
import { AppBreadcrumbs } from '../../components/AppBreadcrumbs';
import { PageContainer } from '../../components/PageContainer';
import { PageHeader } from '../../components/PageHeader';
import { usePageCategory } from '../../hooks/usePageCategory';
import { AppRoutes } from '../../common/app-routes';
import { Button } from '@mantine/core';
import { useButtonVariant } from '../../hooks/useButtonVariant';
import { Link } from 'react-router-dom';
import { useGetFlashcardSets } from '../../hooks/api/useFlashcardSetsApi';
import { CenteredLoader } from '../../components/CenteredLoader';
import { FlashcardSetList } from '../../components/FlashcardSetList';
import { NotFound } from '../../components/NotFound';
import { useDocumentTitle } from '@mantine/hooks';
import { useAppStore } from '../../hooks/useAppStore';

export const FlashcardSetsPage = () => {
  const isLoggedIn = useAppStore((state) => state.isLoggedIn);
  const { t } = useTranslation();
  const buttonVariant = useButtonVariant();
  usePageCategory('flashcards');
  useDocumentTitle(t('FlashcardSet.DocumentTitle.FlashcardSets'));
  const { data: flashcardSets, isLoading: areFlashcardSetsLoading } = useGetFlashcardSets();

  if (areFlashcardSetsLoading) {
    return <CenteredLoader />;
  }

  if (!flashcardSets) {
    return <NotFound />;
  }

  return (
    <PageContainer>
      <PageHeader>
        <AppBreadcrumbs items={[{ title: t('FlashcardSet.Title.Flashcards'), to: AppRoutes.FlashcardSets }]} />
        {isLoggedIn && (
          <Button variant={buttonVariant} component={Link} to={AppRoutes.NewFlashcardSet}>
            {t('FlashcardSet.Action.NewFlashcardSet')}
          </Button>
        )}
      </PageHeader>

      <FlashcardSetList flashcardSets={flashcardSets} />
    </PageContainer>
  );
};
