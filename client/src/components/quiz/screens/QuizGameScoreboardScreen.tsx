import { Button, Card, Center, Group, Text, Stack, Title } from '@mantine/core';
import { QuizPlayerDto } from '../../../api/dtos';
import { useButtonVariant } from '../../../hooks/useButtonVariant';
import * as signalR from '@microsoft/signalr';
import { QuizGameHubMethods } from '../../../hooks/api/quizGamesApi';

interface QuizGameScoreboardProps {
  connection: signalR.HubConnection;
  gamePin: string;
  players: QuizPlayerDto[];
  isFinished: boolean;
}

export const QuizGameScoreboardScreen = (props: QuizGameScoreboardProps) => {
  const buttonVariant = useButtonVariant();

  const onNextClick = async () => {
    await props.connection.invoke(QuizGameHubMethods.NextQuestion, props.gamePin);
  };

  return (
    <Card withBorder shadow='sm'>
      <Stack>
        <Center>
          <Title order={3}>{props.isFinished ? 'Congratulations!' : 'Scoreboard'}</Title>
        </Center>
        <Stack gap='xs'>
          {props.players
            .sort((a, b) => b.accumulatedPoints - a.accumulatedPoints)
            .map((player, idx) => (
              <Card key={player.username} p='xs' shadow='none'>
                <Group justify='space-between'>
                  <Text fw={600}>
                    {idx + 1}. {player.username}
                  </Text>
                  <Text fw={600}>{player.accumulatedPoints}</Text>
                </Group>
              </Card>
            ))}
        </Stack>
        <Group justify='end'>
          {!props.isFinished && (
            <Button variant={buttonVariant} onClick={onNextClick}>
              Next
            </Button>
          )}
        </Group>
      </Stack>
    </Card>
  );
};
