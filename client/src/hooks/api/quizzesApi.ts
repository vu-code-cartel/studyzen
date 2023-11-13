import { useTranslation } from 'react-i18next';
import { SERVER_URL, axiosClient } from '../../api/config';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { QueryKeys } from '../../api/query-keys';
import { QuizDto, QuizQuestionDto } from '../../api/dtos';
import { notifications } from '@mantine/notifications';
import { CreateQuizDto } from '../../api/requests';

const API_URL = `${SERVER_URL}/Quizzes`;

export const useCreateQuiz = () => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (dto: CreateQuizDto) => {
      await axiosClient.post(API_URL, dto);
    },
    onSuccess: () => {
      notifications.show({
        message: t('Quiz.Notification.QuizCreatedSuccessfully'),
        withBorder: true,
        withCloseButton: true,
        color: 'teal',
      });

      queryClient.invalidateQueries([QueryKeys.GetQuizzes]);
    },
    onError: () => {
      notifications.show({
        message: t('Quiz.Notification.FailedToCreateQuiz'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });
    },
  });
};

export const useGetQuiz = (quizId: number | null) => {
  const { t } = useTranslation();

  return useQuery([QueryKeys.GetQuiz, quizId], async () => {
    if (!quizId) {
      return null;
    }

    try {
      const response = await axiosClient.get<QuizDto>(`${API_URL}/${quizId}`);
      return response.data;
    } catch {
      notifications.show({
        message: t('Quiz.Notification.QuizNotFound'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });

      return null;
    }
  });
};

export const useGetQuizzes = () => {
  const { t } = useTranslation();

  return useQuery([QueryKeys.GetQuizzes], async () => {
    try {
      const response = await axiosClient.get<QuizDto[]>(`${API_URL}`);
      return response.data;
    } catch {
      notifications.show({
        message: t('Quiz.Notification.FailedToLoadQuizzes'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });

      return [];
    }
  });
};

export const useGetQuizQuestions = (quizId: number | null) => {
  const { t } = useTranslation();

  return useQuery([QueryKeys.GetQuizQuestions, quizId], async () => {
    try {
      const response = await axiosClient.get<QuizQuestionDto[]>(`${API_URL}/${quizId}/Questions`);
      return response.data;
    } catch {
      notifications.show({
        message: t('Quiz.Notification.FailedToLoadQuizQuestions'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });

      return [];
    }
  });
};
