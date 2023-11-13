import { useTranslation } from 'react-i18next';
import { AppBreadcrumbs } from '../../components/AppBreadcrumbs';
import { PageContainer } from '../../components/PageContainer';
import { PageHeader } from '../../components/PageHeader';
import { usePageCategory } from '../../hooks/usePageCategory';
import { AppRoutes, getQuizRoute } from '../../common/app-routes';
import { Button, Card, Group, SimpleGrid } from '@mantine/core';
import { useButtonVariant } from '../../hooks/useButtonVariant';
import { useGetQuizzes } from '../../hooks/api/quizzesApi';
import { CenteredLoader } from '../../components/CenteredLoader';
import { Link, useNavigate } from 'react-router-dom';
import { EnterQuizGameModal } from '../../components/quiz/EnterQuizGameModal';
import { useDisclosure } from '@mantine/hooks';

export const QuizzesPage = () => {
  const { t } = useTranslation();
  const buttonVariant = useButtonVariant();
  usePageCategory('quizzes');
  const { data: quizzes, isLoading: areQuizzesLoading } = useGetQuizzes();
  const [isModalOpen, { open: openModal, close: closeModal }] = useDisclosure(false);
  const navigate = useNavigate();

  return (
    <PageContainer>
      <EnterQuizGameModal isOpen={isModalOpen} close={closeModal} />

      <PageHeader>
        <AppBreadcrumbs items={[{ title: t('Quiz.Title.Quizzes'), to: AppRoutes.Quizzes }]} />
        <Group>
          <Button variant='outline' onClick={openModal}>
            {t('QuizGame.Button.Join')}
          </Button>
          <Button variant={buttonVariant} onClick={() => navigate(AppRoutes.NewQuiz)}>
            {t('Quiz.Button.New')}
          </Button>
        </Group>
      </PageHeader>

      {areQuizzesLoading && <CenteredLoader />}

      {quizzes && (
        <SimpleGrid cols={3}>
          {quizzes.map((quiz) => (
            <Card withBorder key={quiz.id} component={Link} to={getQuizRoute(quiz.id, quiz.title)}>
              {quiz.title}
            </Card>
          ))}
        </SimpleGrid>
      )}
    </PageContainer>
  );
};
