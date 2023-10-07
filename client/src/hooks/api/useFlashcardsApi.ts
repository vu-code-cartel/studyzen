import { useTranslation } from 'react-i18next';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { SERVER_URL, axiosClient } from '../../api/config';
import { FlashcardDto } from '../../api/dtos';
import { CreateFlashcardRequest, UpdateFlashcardDto } from '../../api/requests';
import { QueryKeys } from '../../api/query-keys';
import { notifications } from '@mantine/notifications';

const FLASHCARDS_API_URL = `${SERVER_URL}/flashcards`;

export const useCreateFlashcard = () => {
  const queryClient = useQueryClient();
  const { t } = useTranslation();

  return useMutation({
    mutationFn: async (request: CreateFlashcardRequest) => {
      await axiosClient.post(FLASHCARDS_API_URL, request);
    },
    onSuccess: (_, request) => {
      notifications.show({
        message: t('Flashcard.Notification.FlashcardCreatedSuccessfully'),
        withBorder: true,
        withCloseButton: true,
        color: 'teal',
      });

      queryClient.invalidateQueries([QueryKeys.GetFlashcards, request.flashcardSetId]);
    },
    onError: () => {
      notifications.show({
        message: t('Flashcard.Notification.FailedToCreateFlashcard'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });
    },
  });
};

export const useGetFlashcard = (flashcardId: number | null) => {
  const { t } = useTranslation();

  return useQuery([QueryKeys.GetFlashcard, flashcardId], async () => {
    if (!flashcardId) {
      return null;
    }

    try {
      const response = await axiosClient.get<FlashcardDto>(`${FLASHCARDS_API_URL}/${flashcardId}`);
      return response.data;
    } catch {
      notifications.show({
        message: t('Flashcard.Notification.FailedToGetFlashcard'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });

      return null;
    }
  });
};

export const useGetFlashcards = (flashcardSetId: number | null) => {
  const { t } = useTranslation();

  return useQuery([QueryKeys.GetFlashcards, flashcardSetId], async () => {
    if (!flashcardSetId) {
      return null;
    }

    try {
      const response = await axiosClient.get<FlashcardDto[]>(`${FLASHCARDS_API_URL}?flashcardSetId=${flashcardSetId}`);
      return response.data;
    } catch {
      notifications.show({
        message: t('Flashcard.Notification.FailedToGetFlashcards'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });

      return [];
    }
  });
};

export const useUpdateFlashcard = () => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async ({ flashcardId, dto }: { flashcardId: number; dto: UpdateFlashcardDto }) => {
      await axiosClient.patch(`${FLASHCARDS_API_URL}/${flashcardId}`, dto);
    },
    onSuccess: (_, { flashcardId: flashcardSetId }) => {
      notifications.show({
        message: t('Flashcard.Notification.FlashcardUpdatedSuccessfully'),
        withBorder: true,
        withCloseButton: true,
        color: 'teal',
      });

      queryClient.invalidateQueries([QueryKeys.GetFlashcards, flashcardSetId]);
    },
    onError: () => {
      notifications.show({
        message: t('Flashcard.Notification.FailedToUpdateFlashcard'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });
    },
  });
};

export const useDeleteFlashcard = () => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async ({ flashcardSetId, flashcardId }: { flashcardSetId: number; flashcardId: number }) => {
      await axiosClient.delete(`${FLASHCARDS_API_URL}/${flashcardId}`);
    },
    onSuccess(_, { flashcardSetId, flashcardId }) {
      notifications.show({
        message: t('Flashcard.Notification.FlashcardDeletedSuccessfully'),
        withBorder: true,
        withCloseButton: true,
        color: 'teal',
      });

      queryClient.invalidateQueries([QueryKeys.GetFlashcard, flashcardId]);
      queryClient.invalidateQueries([QueryKeys.GetFlashcards, flashcardSetId]);
    },
    onError() {
      notifications.show({
        message: t('Flashcard.Notification.FailedToDeleteFlashcard'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });
    },
  });
};
