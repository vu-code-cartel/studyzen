import { Button, Card, Center, Group, Radio, Stack, Title } from '@mantine/core';
import { QuizGameQuestionDto } from '../../../api/dtos';
import { useState } from 'react';
import { useButtonVariant } from '../../../hooks/useButtonVariant';
import Countdown from 'react-countdown';
import * as signalR from '@microsoft/signalr';
import { QuizGameHubMethods } from '../../../hooks/api/quizGamesApi';

interface QuizGameQuestionScreenProps {
  question: QuizGameQuestionDto;
  answerIds: number[] | null;
  connection: signalR.HubConnection;
  gamePin: string;
}

export const QuizGameQuestionScreen = (props: QuizGameQuestionScreenProps) => {
  const [checkedAnswerIds, setCheckedAnswerIds] = useState<number[]>([]);
  const [isAnswerSubmitted, setIsAnswerSubmitted] = useState<boolean>(false);
  const [answerTimeout] = useState<number>(Date.now() + props.question.timeLimitInSeconds * 1000);
  const buttonVariant = useButtonVariant();

  const onChoose = (answerId: number) => {
    if (isAnswerSubmitted) {
      return;
    }

    setCheckedAnswerIds((prev) => (prev.includes(answerId) ? prev.filter((x) => x !== answerId) : [...prev, answerId]));
  };

  const onAnswerSubmit = async () => {
    if (isAnswerSubmitted) {
      return;
    }

    setIsAnswerSubmitted(true);
    await props.connection.invoke(QuizGameHubMethods.SubmitAnswer, props.gamePin, checkedAnswerIds);
  };

  const QuizQuestionTimer = ({ seconds }: { seconds: number }) => {
    return <Title order={3}>{seconds}</Title>;
  };

  const onNextClick = async () => {
    await props.connection.invoke(QuizGameHubMethods.SendScoreboard, props.gamePin);
  };

  return (
    <Center h='100%'>
      <Stack w='100%' mb='xl'>
        <Group justify='space-between'>
          <Title order={4}>{props.question.question}</Title>
          <Countdown
            date={props.answerIds ? 0 : answerTimeout}
            onComplete={onAnswerSubmit}
            renderer={QuizQuestionTimer}
          />
        </Group>
        <Card withBorder>
          <Stack>
            <Stack>
              {props.question.choices.map((choice) => (
                <Radio
                  label={choice.answer}
                  value={choice.answer}
                  checked={checkedAnswerIds.includes(choice.id) || props.answerIds !== null}
                  color={
                    props.answerIds
                      ? props.answerIds.includes(choice.id)
                        ? 'teal'
                        : 'pink'
                      : isAnswerSubmitted
                      ? 'yellow'
                      : undefined
                  }
                  onClick={() => onChoose(choice.id)}
                  onChange={() => {}}
                  key={choice.id}
                />
              ))}
            </Stack>
            <Button variant={buttonVariant} onClick={onAnswerSubmit} disabled={isAnswerSubmitted}>
              Answer
            </Button>
          </Stack>
        </Card>
        <Group justify='end'>
          <Button variant={buttonVariant} onClick={onNextClick}>
            Next
          </Button>
        </Group>
      </Stack>
    </Center>
  );
};
