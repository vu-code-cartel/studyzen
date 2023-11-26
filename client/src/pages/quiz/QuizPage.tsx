import { useTranslation } from 'react-i18next';
import { AppBreadcrumbs } from '../../components/AppBreadcrumbs';
import { PageContainer } from '../../components/PageContainer';
import { PageHeader } from '../../components/PageHeader';
import { AppRoutes, getQuizGameRoomRoute, getQuizRoute } from '../../common/app-routes';
import { usePageCategory } from '../../hooks/usePageCategory';
import { useNavigate, useParams } from 'react-router-dom';
import { useAddQuestionToQuiz, useGetQuiz, useGetQuizQuestions } from '../../hooks/api/quizzesApi';
import { getIdFromSlug } from '../../common/utils';
import { CenteredLoader } from '../../components/CenteredLoader';
import {
  Accordion,
  Button,
  Card,
  Radio,
  Stack,
  useMantineTheme,
  Text,
  Group,
  TextInput,
  Divider,
  NumberInput,
} from '@mantine/core';
import { useButtonVariant } from '../../hooks/useButtonVariant';
import { useCreateGame } from '../../hooks/api/quizGamesApi';
import { QuizDto } from '../../api/dtos';
import { useAppStore } from '../../hooks/useAppStore';
import { useState } from 'react';
import { useForm } from '@mantine/form';
import { CreateQuizQuestionDto } from '../../api/requests';

type QuizChoice = {
  answer: string;
  isCorrect: boolean;
};

type NewQuizQuestionForm = {
  question: string;
  choice: string;
  timeLimitInSeconds: number;
};

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
  const [choices, setChoices] = useState<QuizChoice[]>([]);
  const initialValues: NewQuizQuestionForm = {
    question: '',
    choice: '',
    timeLimitInSeconds: 15,
  };
  const form = useForm<NewQuizQuestionForm>({
    initialValues,
    validate: {
      question: (value) => (value ? undefined : 'Question is required'),
    },
  });
  const addQuestion = useAddQuestionToQuiz();

  const onCreateGameClick = async (quiz: QuizDto) => {
    const game = await createGame.mutateAsync(quiz.id);
    navigate(getQuizGameRoomRoute(game.pin));
  };

  const onAddQuestionClick = async (values: NewQuizQuestionForm) => {
    if (!quiz?.id) {
      return;
    }

    const correctAnswers = choices.filter((c) => c.isCorrect).map((c) => c.answer);
    const incorrectAnswers = choices.filter((c) => !c.isCorrect).map((c) => c.answer);

    if (correctAnswers.length + incorrectAnswers.length <= 0) {
      return;
    }

    const dto: CreateQuizQuestionDto = {
      question: values.question,
      correctAnswers,
      incorrectAnswers,
      timeLimitInSeconds: values.timeLimitInSeconds,
    };

    await addQuestion.mutateAsync({ quizId: quiz.id, dto });
    form.setValues(initialValues);
    setChoices([]);
  };

  const onAddChoiceClick = () => {
    if (!form.values.choice) {
      form.setFieldError('choice', 'Choice is required');
    } else if (choices.findIndex((c) => c.answer === form.values.choice) !== -1) {
      form.setFieldError('choice', 'Such choice already exists');
    } else {
      setChoices((prev) => [...prev, { answer: form.values.choice, isCorrect: false }]);
    }
  };

  const onSetAnswer = (choice: QuizChoice) => {
    const nextIsCorrect = !choice.isCorrect;

    setChoices((prev) =>
      prev.map((c) => {
        if (c === choice) {
          c.isCorrect = nextIsCorrect;
        }

        return c;
      }),
    );
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
          <Card mb='md' withBorder>
            <form onSubmit={form.onSubmit(onAddQuestionClick)}>
              <Stack>
                <Text>Add question</Text>
                <TextInput label='Question' {...form.getInputProps('question')} />
                <NumberInput
                  label='Time limit in seconds'
                  min={3}
                  max={600}
                  {...form.getInputProps('timeLimitInSeconds')}
                />
                <Stack>
                  <TextInput label='Choice' {...form.getInputProps('choice')} />
                  <Group justify='end'>
                    <Button variant={buttonVariant} onClick={onAddChoiceClick}>
                      Add choice
                    </Button>
                  </Group>
                </Stack>
                {choices.length > 0 && <Divider />}
                {choices.map((choice) => (
                  <Radio
                    label={choice.answer}
                    key={choice.answer}
                    color={choice.isCorrect ? 'teal' : undefined}
                    checked={choice.isCorrect}
                    onClick={() => onSetAnswer(choice)}
                    onChange={() => {}}
                  />
                ))}
                <Divider />
                <Button variant={buttonVariant} type='submit'>
                  Add question
                </Button>
              </Stack>
            </form>
          </Card>
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
