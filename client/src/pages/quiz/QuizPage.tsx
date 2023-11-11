import { useTranslation } from 'react-i18next';
import { AppBreadcrumbs } from '../../components/AppBreadcrumbs';
import { PageContainer } from '../../components/PageContainer';
import { PageHeader } from '../../components/PageHeader';
import { AppRoutes, getQuizGameRoomRoute, getQuizRoute } from '../../common/app-routes';
import { usePageCategory } from '../../hooks/usePageCategory';
import { useNavigate, useParams } from 'react-router-dom';
import { useGetQuiz, useGetQuizQuestions } from '../../hooks/api/quizzesApi';
import { getIdFromSlug } from '../../common/utils';
import { CenteredLoader } from '../../components/CenteredLoader';
import { Accordion, Button, Radio, Stack, useMantineTheme } from '@mantine/core';
import { useButtonVariant } from '../../hooks/useButtonVariant';
import { useCreateGame } from '../../hooks/api/quizGamesApi';
import { QuizDto } from '../../api/dtos';
import { useAppStore } from '../../hooks/useAppStore';

export const QuizPage = () => {
  const { t } = useTranslation();
  const { quizIdWithSlug } = useParams();
  const { data: quiz, isLoading: isQuizLoading } = useGetQuiz(getIdFromSlug(quizIdWithSlug));
  const { data: questions, isLoading: areQuestionsLoading } = useGetQuizQuestions(getIdFromSlug(quizIdWithSlug));
  const buttonVariant = useButtonVariant();
  const createGame = useCreateGame();
  const navigate = useNavigate();
  const colorScheme = useAppStore((state) => state.colorScheme);
  usePageCategory('quizzes');
  const theme = useMantineTheme();

  const onCreateGameClick = async (quiz: QuizDto) => {
    const game = await createGame.mutateAsync(quiz.id);
    navigate(getQuizGameRoomRoute(game.pin));
  };

  return (
    <PageContainer>
      {isQuizLoading && <CenteredLoader />}
      {quiz && (
        <>
          <PageHeader>
            <AppBreadcrumbs
              items={[
                { title: t('Quiz.Title.Quizzes'), to: AppRoutes.Quizzes },
                { title: quiz.title, to: getQuizRoute(quiz.id, quiz.title) },
              ]}
            />
            <Button variant={buttonVariant} onClick={() => onCreateGameClick(quiz)}>
              {t('QuizGame.Button.Create')}
            </Button>
          </PageHeader>
          {areQuestionsLoading && <CenteredLoader />}
          {questions && (
            <Accordion variant='contained'>
              {questions.map((question) => (
                <Accordion.Item
                  key={question.id}
                  value={question.question}
                  bg={colorScheme === 'dark' ? theme.colors.dark[6] : theme.white}
                >
                  <Accordion.Control>{question.question}</Accordion.Control>
                  <Accordion.Panel>
                    <Stack>
                      {question.choices.map((choice) => (
                        <Radio
                          label={choice.answer}
                          value={choice.answer}
                          checked={choice.isCorrect}
                          color={choice.isCorrect ? 'teal' : undefined}
                          key={choice.id}
                          onChange={() => {}} // to prevent error
                        />
                      ))}
                    </Stack>
                  </Accordion.Panel>
                </Accordion.Item>
              ))}
            </Accordion>
          )}
        </>
      )}
    </PageContainer>
  );
};
