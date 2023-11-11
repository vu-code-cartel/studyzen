import { PageContainer } from '../../components/PageContainer';
import { useParams } from 'react-router-dom';
import { QUIZ_GAME_HUB_URL, QuizGameHubMethods, useGetQuizGame } from '../../hooks/api/quizGamesApi';
import { CenteredLoader } from '../../components/CenteredLoader';
import { useEffect, useRef, useState } from 'react';
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
  const connection = useRef<signalR.HubConnection | null>(null);
  const [isConnected, setIsConnected] = useState<boolean>(false);
  const [players, setPlayers] = useState<QuizPlayerDto[]>([]);
  const [question, setQuestion] = useState<QuizGameQuestionDto | null>(null);
  const { data: game, isLoading: isGameLoading, isError } = useGetQuizGame(gamePin ?? null);

  const stopConnection = async () => {
    if (connection.current?.state === signalR.HubConnectionState.Connected) {
      await connection.current.stop();
    }
  };

  useEffect(() => {
    const initializeConnection = async () => {
      await connection.current?.start();
      setIsConnected(true);
    };

    if (!connection.current) {
      connection.current = buildHubConnection(QUIZ_GAME_HUB_URL);
      initializeConnection();
    }

    return () => {
      if (connection.current) {
        stopConnection();
      }
    };
  }, []);

  useEffect(() => {
    if (!isConnected || !connection.current || !game) {
      return;
    }

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
      setPlayers((prev) => prev.filter((p) => p.username !== player.username));
    };

    const onQuestionReceive = (question: QuizGameQuestionDto) => {
      setQuestion(question);
    };

    const initialize = async (connection: signalR.HubConnection) => {
      connection.on(QuizGameHubMethods.OnPlayerJoin, onPlayerJoin);
      connection.on(QuizGameHubMethods.OnPlayerLeave, onPlayerLeave);
      connection.on(QuizGameHubMethods.OnQuestionReceive, onQuestionReceive);

      await connection.invoke(QuizGameHubMethods.ConnectToGame, game.pin);

      const players = await connection.invoke<QuizPlayerDto[]>(QuizGameHubMethods.GetPlayers, game.pin);
      setPlayers(players);
    };

    if (game.state === QuizGameState.NotStarted) {
      initialize(connection.current);
    } else {
      stopConnection();
      alert('quiz already started');
      // TODO: show disconnected and disconnect
    }
  }, [isConnected, connection, game]);

  return (
    <PageContainer>
      {isError ? (
        <NotFound />
      ) : isGameLoading || !game || !isConnected ? (
        <CenteredLoader text={t('QuizGame.Title.Connecting')} />
      ) : (
        <>
          <JoinQuizGameModal gamePin={game.pin} connectionId={connection.current!.connectionId!} />
          <QuizWaitingRoom game={game} players={players} />
          {question && question.question}
        </>
      )}
    </PageContainer>
  );
};
