import { AppBreadcrumbs } from '../../components/AppBreadcrumbs';
import { PageContainer } from '../../components/PageContainer';
import { PageHeader } from '../../components/PageHeader';
import { usePageCategory } from '../../hooks/usePageCategory';
import { useTranslation } from 'react-i18next';
import { AppRoutes } from '../../common/app-routes';
import { FlashcardSetForm } from '../../components/FlashcardSetForm';
import { CreateFlashcardSetRequest } from '../../api/requests';
import { useCreateFlashcardSet } from '../../hooks/api/useFlashcardSetsApi';
import { useDocumentTitle } from '@mantine/hooks';

export const NewFlashcardSetPage = () => {
  const { t } = useTranslation();
  usePageCategory('flashcards');
  useDocumentTitle(t('FlashcardSet.DocumentTitle.NewFlashcardSet'));
  const createFlashcardSetMutation = useCreateFlashcardSet();

  const onCreateFlashcardSetClick = async (request: CreateFlashcardSetRequest) => {
    await createFlashcardSetMutation.mutateAsync(request);
  };

  return (
    <PageContainer>
      <PageHeader>
        <AppBreadcrumbs
          items={[
            { title: t('FlashcardSet.Title.Flashcards'), to: AppRoutes.FlashcardSets },
            { title: t('FlashcardSet.Title.NewFlashcardSet'), to: AppRoutes.NewFlashcardSet },
          ]}
        />
      </PageHeader>

      <FlashcardSetForm onSubmit={onCreateFlashcardSetClick} />
    </PageContainer>
  );
};
