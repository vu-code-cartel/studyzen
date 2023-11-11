import { PageContainer } from '../../components/PageContainer';
import { useParams } from 'react-router-dom';
import { QUIZ_GAME_HUB_URL, QuizGameHubMethods, useGetQuizGame } from '../../hooks/api/quizGamesApi';
import { CenteredLoader } from '../../components/CenteredLoader';
import { useEffect, useState } from 'react';
import { QuizGameQuestionDto, QuizGameState, QuizPlayerDto } from '../../api/dtos';
import * as signalR from '@microsoft/signalr';
import { JoinQuizGameModal } from '../../components/quiz/JoinQuizGameModal';
import { QuizWaitingRoom } from '../../components/quiz/QuizWaitingRoom';
import { useTranslation } from 'react-i18next';
import { NotFound } from '../../components/NotFound';
import { buildHubConnection } from '../../common/utils';

export const QuizRoomPage = () => {
  const { gamePin } = useParams();
  const { t } = useTranslation();
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [players, setPlayers] = useState<QuizPlayerDto[]>([]);
  const [question, setQuestion] = useState<QuizGameQuestionDto | null>(null);
  const [errorScreen, setErrorScreen] = useState<React.ReactNode | null>(null);
  const { data: game, isLoading: isGameLoading, isError: isGameLoadError } = useGetQuizGame(gamePin ?? null);

  useEffect(() => {
    if (!game) {
      return;
    }

    if (game.state !== QuizGameState.NotStarted) {
      setErrorScreen(<div>game has already started</div>);
      return;
    }

    const hubConnection = buildHubConnection(QUIZ_GAME_HUB_URL);

    const onPlayerJoin = (newPlayer: QuizPlayerDto) => {
      setPlayers((prev) => {
        const playerExists = prev.findIndex((p) => newPlayer.username === p.username) !== -1;
        if (!playerExists) {
          return [...prev, newPlayer];
        } else {
          return prev;
        }
      });
    };

    const onPlayerLeave = (player: QuizPlayerDto) => {
      console.log(player);
      setPlayers((prev) => prev.filter((p) => p.username !== player.username));
    };

    const onQuestionReceive = (question: QuizGameQuestionDto) => {
      setQuestion(question);
    };

    const initialize = async () => {
      await hubConnection.start();

      hubConnection.on(QuizGameHubMethods.OnPlayerJoin, onPlayerJoin);
      hubConnection.on(QuizGameHubMethods.OnPlayerLeave, onPlayerLeave);
      hubConnection.on(QuizGameHubMethods.OnQuestionReceive, onQuestionReceive);

      await hubConnection.invoke(QuizGameHubMethods.ConnectToGame, game.pin);

      const players = await hubConnection.invoke<QuizPlayerDto[]>(QuizGameHubMethods.GetPlayers, game.pin);
      setPlayers(players);

      setConnection(hubConnection);
    };

    const uninitialize = async () => {
      if (hubConnection.state === signalR.HubConnectionState.Connected) {
        await hubConnection.stop();
      }
    };

    initialize();

    return () => {
      uninitialize();
    };
  }, [game]);

  return (
    <PageContainer>
      {isGameLoadError ? (
        <NotFound />
      ) : errorScreen ? (
        errorScreen
      ) : (
        <>
          <JoinQuizGameModal gamePin={game?.pin} connectionId={connection ? connection.connectionId! : undefined} />
          {isGameLoading || !game || !connection ? (
            <CenteredLoader text={t('QuizGame.Title.Connecting')} />
          ) : (
            <QuizWaitingRoom game={game} players={players} />
          )}
          {question && question.question}
        </>
      )}
    </PageContainer>
  );
};
