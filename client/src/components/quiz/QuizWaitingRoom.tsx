import { Badge, Card, Center, Divider, Group, Stack, Title, Text, Button } from '@mantine/core';
import { useTranslation } from 'react-i18next';
import { QuizGameDto, QuizPlayerDto } from '../../api/dtos';
import { useButtonVariant } from '../../hooks/useButtonVariant';
import { useStartQuizGame } from '../../hooks/api/quizGamesApi';

interface QuizWaitingRoomProps {
  game: QuizGameDto;
  players: QuizPlayerDto[];
}

export const QuizWaitingRoom = (props: QuizWaitingRoomProps) => {
  const { t } = useTranslation();
  const buttonVariant = useButtonVariant();
  const startGame = useStartQuizGame();

  const onStartGameClick = async () => {
    await startGame.mutateAsync(props.game.pin);
  };

  return (
    <Stack mt='sm'>
      <Center mb='sm'>
        <Stack align='center' gap={0}>
          <Title order={3} fw={500}>
            {t('QuizGame.Field.GamePin.Label')}
          </Title>
          <Title order={1}>{props.game.pin}</Title>
        </Stack>
      </Center>
      <Card shadow='sm' withBorder>
        <Stack>
          <Group justify='space-between'>
            <Text fw={600} size='xl'>
              {t('QuizGame.Title.GetReadyForGame', { quizTitle: props.game.quiz.title })}
            </Text>
            <Button variant={buttonVariant} color='indigo' onClick={onStartGameClick}>
              {t('QuizGame.Button.Start')}
            </Button>
          </Group>
          <Divider />
          {t('QuizGame.Title.PlayerCount', { count: props.players.length })}
          <Group>
            {props.players.map((player) => (
              <Badge
                variant='dot'
                p='lg'
                size='xl'
                fw={500}
                style={{ textTransform: 'none' }}
                color='teal'
                key={player.username}
              >
                {player.username}
              </Badge>
            ))}
          </Group>
        </Stack>
      </Card>
    </Stack>
  );
};
