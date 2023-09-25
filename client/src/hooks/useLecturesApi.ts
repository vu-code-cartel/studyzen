import { useTranslation } from 'react-i18next';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { SERVER_URL, axiosClient } from '../api/config';
import { LectureDto } from '../api/dtos';
import { CreateLectureRequest, UpdateLectureRequest } from '../api/requests';
import { notifications } from '@mantine/notifications';
import { formatUserActionStamp } from '../common/utils';

const LECTURES_API_URL = `${SERVER_URL}/lectures`;

export const useCreateLecture = () => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (request: CreateLectureRequest) => {
      await axiosClient.post(LECTURES_API_URL, request);
    },
    onSuccess: (_, request) => {
      notifications.show({
        message: t('Lecture.Notification.LectureCreatedSuccessfully'),
        withBorder: true,
        withCloseButton: true,
        color: 'teal',
      });

      queryClient.invalidateQueries(['getLectures', request.courseId]);
    },
    onError: () => {
      notifications.show({
        message: t('Lecture.Notification.FailedToCreateLecture'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });
    },
  });
};

export const useGetLecture = (lectureId: number | null) => {
  const { t } = useTranslation();

  return useQuery(['getLecture', lectureId], async () => {
    if (!lectureId) {
      return null;
    }

    try {
      const response = await axiosClient.get<LectureDto>(`${LECTURES_API_URL}/${lectureId}`);
      return formatLectureDto(response.data);
    } catch {
      notifications.show({
        message: t('Lecture.Notification.FailedToGetLecture'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });

      return null;
    }
  });
};

export const useGetLectures = (courseId: number | null) => {
  const { t } = useTranslation();

  return useQuery(['getLectures', courseId], async () => {
    if (!courseId) {
      return null;
    }

    try {
      const response = await axiosClient.get<LectureDto[]>(`${LECTURES_API_URL}?courseId=${courseId}`);
      response.data = response.data.map((dto) => formatLectureDto(dto));
      return response.data;
    } catch {
      notifications.show({
        message: t('Lecture.Notification.FailedToGetLectures'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });

      return [];
    }
  });
};

export const useUpdateLecture = () => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async ({
      // eslint-disable-next-line @typescript-eslint/no-unused-vars
      courseId,
      lectureId,
      request,
    }: {
      courseId: number;
      lectureId: number;
      request: UpdateLectureRequest;
    }) => {
      await axiosClient.patch(`${LECTURES_API_URL}/${lectureId}`, request);
    },
    onSuccess: (_, { courseId, lectureId }) => {
      notifications.show({
        message: t('Lecture.Notification.LectureUpdatedSuccessfully'),
        withBorder: true,
        withCloseButton: true,
        color: 'teal',
      });

      queryClient.invalidateQueries(['getLectures', courseId]);
      queryClient.invalidateQueries(['getLecture', lectureId]);
    },
    onError: () => {
      notifications.show({
        message: t('Lecture.Notification.FailedToUpdateLecture'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });
    },
  });
};

export const useDeleteLecture = () => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  return useMutation({
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    mutationFn: async ({ courseId, lectureId }: { courseId: number; lectureId: number }) => {
      await axiosClient.delete(`${LECTURES_API_URL}/${lectureId}`);
    },
    onSuccess: (_, { courseId }) => {
      notifications.show({
        message: t('Lecture.Notification.LectureDeletedSuccessfully'),
        withBorder: true,
        withCloseButton: true,
        color: 'teal',
      });

      queryClient.invalidateQueries(['getLectures', courseId]);
    },
    onError: () => {
      notifications.show({
        message: t('Lecture.Notification.FailedToDeleteLecture'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });
    },
  });
};

const formatLectureDto = (dto: LectureDto): LectureDto => ({
  ...dto,
  createdBy: formatUserActionStamp(dto.createdBy),
  updatedBy: formatUserActionStamp(dto.updatedBy),
});
