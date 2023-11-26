import { PageContainer } from '../../components/PageContainer';
import { useParams } from 'react-router-dom';
import { QUIZ_GAME_HUB_URL, QuizGameHubMethods, useGetQuizGame } from '../../hooks/api/quizGamesApi';
import { useEffect, useState } from 'react';
import { QuizGameQuestionDto, QuizGameState, QuizPlayerDto } from '../../api/dtos';
import * as signalR from '@microsoft/signalr';
import { NotFound } from '../../components/NotFound';
import { buildHubConnection } from '../../common/utils';
import { TooLateForQuizGameScreen } from '../../components/quiz/screens/QuizGameAlreadyStartedScreen';
import { InitialQuizGameScreen } from '../../components/quiz/screens/InitialQuizGameScreen';
import { QuizGameQuestionScreen } from '../../components/quiz/screens/QuizGameQuestionScreen';
import { QuizGameScoreboardScreen } from '../../components/quiz/screens/QuizGameScoreboardScreen';
import { usePageCategory } from '../../hooks/usePageCategory';
import Confetti from 'react-confetti';

enum QuizRoomScreen {
  WaitingForUserToJoin,
  GameNotFound,
  TooLateForGame,
  WaitingForGameToStart,
  GameStarting,
  Question,
  Scoreboard,
  Finish,
}

export const QuizRoomPage = () => {
  const { gamePin } = useParams();
  usePageCategory('quizzes');
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [players, setPlayers] = useState<QuizPlayerDto[]>([]);
  const [question, setQuestion] = useState<QuizGameQuestionDto | null>(null);
  const [answerIds, setAnswerIds] = useState<number[] | null>(null);
  const { data: game, isLoading: isGameLoading, isError: isGameLoadError } = useGetQuizGame(gamePin ?? null);
  const [currentScreen, setCurrentScreen] = useState<QuizRoomScreen>(QuizRoomScreen.WaitingForUserToJoin);

  const closeConnection = async (connection: signalR.HubConnection) => {
    if (connection.state === signalR.HubConnectionState.Connected) {
      await connection.stop();
    }
  };

  useEffect(() => {
    if (!game) {
      return;
    }

    if (game.state !== QuizGameState.NotStarted) {
      setCurrentScreen(QuizRoomScreen.TooLateForGame);
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
      setPlayers((prev) => prev.filter((p) => p.username !== player.username));
    };

    const onGameStart = () => {
      setCurrentScreen((prev) =>
        prev === QuizRoomScreen.WaitingForUserToJoin ? QuizRoomScreen.TooLateForGame : QuizRoomScreen.GameStarting,
      );
    };

    const onQuestionReceive = (question: QuizGameQuestionDto) => {
      setQuestion(question);
      setCurrentScreen(QuizRoomScreen.Question);
    };

    const onAnswerReceive = (answerIds: number[]) => {
      setAnswerIds(answerIds);
    };

    const onScoreboardReceive = (players: QuizPlayerDto[]) => {
      setPlayers(players);
      setCurrentScreen(QuizRoomScreen.Scoreboard);
      setAnswerIds(null);
      setQuestion(null);
    };

    const onGameFinish = (players: QuizPlayerDto[]) => {
      setPlayers(players);
      setCurrentScreen(QuizRoomScreen.Finish);
      setAnswerIds(null);
      setQuestion(null);
    };

    const initialize = async () => {
      await hubConnection.start();

      hubConnection.on(QuizGameHubMethods.OnPlayerJoin, onPlayerJoin);
      hubConnection.on(QuizGameHubMethods.OnPlayerLeave, onPlayerLeave);
      hubConnection.on(QuizGameHubMethods.OnGameStart, onGameStart);
      hubConnection.on(QuizGameHubMethods.OnQuestionReceive, onQuestionReceive);
      hubConnection.on(QuizGameHubMethods.OnAnswerReceive, onAnswerReceive);
      hubConnection.on(QuizGameHubMethods.OnAnswerReceive, onAnswerReceive);
      hubConnection.on(QuizGameHubMethods.OnScoreboardReceive, onScoreboardReceive);
      hubConnection.on(QuizGameHubMethods.OnGameFinish, onGameFinish);

      await hubConnection.invoke(QuizGameHubMethods.ConnectToGame, game.pin);

      const players = await hubConnection.invoke<QuizPlayerDto[]>(QuizGameHubMethods.GetPlayers, game.pin);
      setPlayers(players);

      setConnection(hubConnection);
    };

    initialize();

    return () => {
      closeConnection(hubConnection);
    };
  }, [game]);

  useEffect(() => {
    if (isGameLoadError || (!isGameLoading && !game)) {
      setCurrentScreen(QuizRoomScreen.GameNotFound);
    }
  }, [isGameLoadError, game, isGameLoading]);

  useEffect(() => {
    if (currentScreen === QuizRoomScreen.GameNotFound || currentScreen === QuizRoomScreen.TooLateForGame) {
      connection && closeConnection(connection);
    }
  }, [currentScreen, connection]);

  return (
    <PageContainer>
      {currentScreen === QuizRoomScreen.WaitingForUserToJoin ||
      currentScreen === QuizRoomScreen.WaitingForGameToStart ? (
        <InitialQuizGameScreen
          players={players}
          game={game ?? null}
          connectionId={connection ? connection.connectionId : null}
          onPlayerJoin={() => setCurrentScreen(QuizRoomScreen.WaitingForGameToStart)}
        />
      ) : currentScreen === QuizRoomScreen.GameNotFound ? (
        <NotFound />
      ) : currentScreen === QuizRoomScreen.TooLateForGame ? (
        <TooLateForQuizGameScreen />
      ) : connection && game ? (
        currentScreen === QuizRoomScreen.Question && question ? (
          <QuizGameQuestionScreen
            question={question}
            answerIds={answerIds}
            gamePin={game.pin}
            connection={connection}
          />
        ) : currentScreen === QuizRoomScreen.Scoreboard || currentScreen === QuizRoomScreen.Finish ? (
          <>
            <QuizGameScoreboardScreen
              gamePin={game.pin}
              players={players}
              connection={connection}
              isFinished={currentScreen === QuizRoomScreen.Finish}
            />
            {currentScreen === QuizRoomScreen.Finish && <Confetti />}
          </>
        ) : null
      ) : null}
    </PageContainer>
  );
};
