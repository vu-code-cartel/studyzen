import { useTranslation } from 'react-i18next';
import { SERVER_URL, axiosClient } from '../../api/config';
import { useMutation, useQuery } from '@tanstack/react-query';
import { JoinQuizGameDto } from '../../api/requests';
import axios, { AxiosError } from 'axios';
import { ErrorCodes, IdentifiableError } from '../../api/errors';
import { notifications } from '@mantine/notifications';
import { QuizGameDto } from '../../api/dtos';
import { QueryKeys } from '../../api/query-keys';

const API_URL = `${SERVER_URL}/QuizGames`;
export const QUIZ_GAME_HUB_URL = `${SERVER_URL}/QuizGameHub`;

export class QuizGameHubMethods {
  public static readonly ConnectToGame = 'ConnectToGame';
  public static readonly GetPlayers = 'GetPlayers';
  public static readonly OnPlayerJoin = 'OnPlayerJoin';
  public static readonly OnPlayerLeave = 'OnPlayerLeave';
  public static readonly OnGameStart = 'OnGameStart';
  public static readonly OnQuestionReceive = 'OnQuestionReceive';
  public static readonly OnAnswerReceive = 'OnAnswerReceive';
  public static readonly SubmitAnswer = 'SubmitAnswer';
  public static readonly SendScoreboard = 'SendScoreboard';
  public static readonly OnScoreboardReceive = 'OnScoreboardReceive';
  public static readonly NextQuestion = 'NextQuestion';
  public static readonly OnGameFinish = 'OnGameFinish';
}

export const useCreateGame = () => {
  const { t } = useTranslation();

  return useMutation({
    mutationFn: async (quizId: number) => {
      const response = await axiosClient.post<QuizGameDto>(`${API_URL}/${quizId}`);
      return response.data;
    },
    onError: () => {
      notifications.show({
        message: t('QuizGame.Notification.FailedToCreateGame'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });
    },
  });
};

export const useQuizJoinGame = () => {
  const { t } = useTranslation();

  return useMutation({
    mutationFn: async (dto: JoinQuizGameDto) => {
      await axiosClient.post(
        `${API_URL}/${dto.gamePin}/join?username=${dto.username}&connectionId=${dto.connectionId}`,
      );
    },
    onError: (error: Error | AxiosError) => {
      let errorMessage = t('QuizGame.Notification.FailedToJoinGame');

      if (axios.isAxiosError(error)) {
        const knownError = error.response?.data as IdentifiableError;
        if (knownError) {
          switch (knownError.errorCode) {
            case ErrorCodes.QuizGameAlreadyStarted:
              errorMessage = t('QuizGame.Notification.GameAlreadyStarted');
              break;
            case ErrorCodes.QuizGameNotFound:
              errorMessage = t('QuizGame.Notification.GameNotFound');
              break;
            case ErrorCodes.UsernameTaken:
              errorMessage = t('QuizGame.Notification.UsernameTaken');
              break;
          }
        }
      }

      notifications.show({
        message: errorMessage,
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });
    },
  });
};

export const useGetQuizGame = (gamePin: string | null, enabled: boolean = true) => {
  const { t } = useTranslation();

  return useQuery(
    [QueryKeys.GetQuizGame, gamePin],
    async () => {
      if (!gamePin) {
        return null;
      }

      try {
        const game = await axiosClient.get<QuizGameDto>(`${API_URL}/${gamePin}`);
        return game.data;
      } catch (error: unknown | AxiosError) {
        let errorMessage = t('QuizGame.Notification.FailedToJoinGame');

        if (axios.isAxiosError(error)) {
          const knownError = error.response?.data as IdentifiableError;
          if (knownError) {
            switch (knownError.errorCode) {
              case ErrorCodes.QuizGameAlreadyStarted:
                errorMessage = t('QuizGame.Notification.GameAlreadyStarted');
                break;
              case ErrorCodes.QuizGameNotFound:
                errorMessage = t('QuizGame.Notification.GameNotFound');
                break;
              case ErrorCodes.UsernameTaken:
                errorMessage = t('QuizGame.Notification.UsernameTaken');
                break;
            }
          }
        }

        notifications.show({
          message: errorMessage,
          withBorder: true,
          withCloseButton: true,
          color: 'red',
        });

        return Promise.reject(error);
      }
    },
    {
      enabled: enabled,
      retry: false,
    },
  );
};

export const useStartQuizGame = () => {
  return useMutation({
    mutationFn: async (gamePin: string) => {
      await axiosClient.post(`${API_URL}/${gamePin}/Start`);
    },
  });
};
