import { useTranslation } from 'react-i18next';
import { AppBreadcrumbs } from '../../components/AppBreadcrumbs';
import { PageContainer } from '../../components/PageContainer';
import { PageHeader } from '../../components/PageHeader';
import { usePageCategory } from '../../hooks/usePageCategory';
import { AppRoutes } from '../../common/app-routes';
import { useForm } from '@mantine/form';
import { CreateQuizDto } from '../../api/requests';
import { Button, Fieldset, Stack, TextInput } from '@mantine/core';
import { useButtonVariant } from '../../hooks/useButtonVariant';
import { useCreateQuiz } from '../../hooks/api/quizzesApi';

export const NewQuizPage = () => {
  const { t } = useTranslation();
  usePageCategory('quizzes');
  const buttonVariant = useButtonVariant();
  const createQuiz = useCreateQuiz();
  const form = useForm<CreateQuizDto>({
    initialValues: {
      title: '',
    },
    validate: {
      title: (value) => (value ? undefined : t('Quiz.Field.Title.Error.Required')),
    },
  });

  const onCreateQuizClick = async (dto: CreateQuizDto) => {
    await createQuiz.mutateAsync(dto);
  };

  return (
    <PageContainer mih='a'>
      <PageHeader>
        <AppBreadcrumbs
          items={[
            { title: t('Quiz.Title.Quizzes'), to: AppRoutes.Quizzes },
            { title: t('Generic.New'), to: AppRoutes.NewQuiz },
          ]}
        />
      </PageHeader>
      <form onSubmit={form.onSubmit(onCreateQuizClick)}>
        <Fieldset>
          <Stack>
            <TextInput label={t('Quiz.Field.Title.Label')} {...form.getInputProps('title')} />
            <Button type='submit' variant={buttonVariant} color='teal' loading={createQuiz.isLoading}>
              {t('Generic.Create')}
            </Button>
          </Stack>
        </Fieldset>
      </form>
    </PageContainer>
  );
};
