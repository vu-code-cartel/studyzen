import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { CreateFlashcardSetRequest, UpdateFlashcardSetDto } from '../../api/requests';
import { SERVER_URL, axiosClient } from '../../api/config';
import { FlashcardSetDto } from '../../api/dtos';
import { notifications } from '@mantine/notifications';
import { useTranslation } from 'react-i18next';
import { QueryKeys } from '../../api/query-keys';

const FLASHCARD_SETS_API_URL = `${SERVER_URL}/FlashcardSets`;

export const useCreateFlashcardSet = () => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (request: CreateFlashcardSetRequest) => {
      await axiosClient.post(FLASHCARD_SETS_API_URL, request);
    },
    onSuccess() {
      notifications.show({
        message: t('FlashcardSet.Notification.FlashcardSetCreatedSuccessfully'),
        withBorder: true,
        withCloseButton: true,
        color: 'teal',
      });

      queryClient.invalidateQueries([QueryKeys.GetFlashcardSets]);
    },
    onError: () => {
      notifications.show({
        message: t('FlashcardSet.Notification.FailedToCreateFlashcardSet'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });
    },
  });
};

export const useGetFlashcardSet = (flashcardSetId: number | null) => {
  const { t } = useTranslation();

  return useQuery([QueryKeys.GetFlashcardSet, flashcardSetId], async () => {
    if (!flashcardSetId) {
      return null;
    }

    try {
      const response = await axiosClient.get<FlashcardSetDto>(`${FLASHCARD_SETS_API_URL}/${flashcardSetId}`);
      return response.data;
    } catch {
      notifications.show({
        message: t('FlashcardSet.Notification.FailedToGetFlashcardSet'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });

      return null;
    }
  });
};

export const useGetFlashcardSets = (lectureId?: number) => {
  const { t } = useTranslation();

  return useQuery([QueryKeys.GetFlashcardSets, lectureId], async () => {
    try {
      const response = await axiosClient.get<FlashcardSetDto[]>(
        `${FLASHCARD_SETS_API_URL}${lectureId ? `?lectureId=${lectureId}` : ''}`,
      );
      return response.data;
    } catch {
      notifications.show({
        message: t('FlashcardSet.Notification.FailedToGetFlashcardSets'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });

      return [];
    }
  });
};

export const useUpdateFlashcardSet = () => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async ({ flashcardSetId, dto }: { flashcardSetId: number; dto: UpdateFlashcardSetDto }) => {
      await axiosClient.patch(`${FLASHCARD_SETS_API_URL}/${flashcardSetId}`, dto);
    },
    onSuccess: (_, { flashcardSetId }) => {
      notifications.show({
        message: t('FlashcardSet.Notification.FlashcardSetUpdatedSuccessfully'),
        withBorder: true,
        withCloseButton: true,
        color: 'teal',
      });

      queryClient.invalidateQueries([QueryKeys.GetFlashcardSets]);
      queryClient.invalidateQueries([QueryKeys.GetFlashcardSet, flashcardSetId]);
    },
    onError: () => {
      notifications.show({
        message: t('FlashcardSet.Notification.FailedToUpdateFlashcardSet'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });
    },
  });
};

export const useDeleteFlashcardSet = () => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (flashcardSetId: number) => {
      await axiosClient.delete(`${FLASHCARD_SETS_API_URL}/${flashcardSetId}`);
    },
    onSuccess() {
      notifications.show({
        message: t('FlashcardSet.Notification.FlashcardSetDeletedSuccessfully'),
        withBorder: true,
        withCloseButton: true,
        color: 'teal',
      });

      queryClient.invalidateQueries([QueryKeys.GetFlashcardSets]);
    },
    onError() {
      notifications.show({
        message: t('FlashcardSet.Notification.FailedToDeleteFlashcardSet'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });
    },
  });
};
