import { useTranslation } from 'react-i18next';
import { SERVER_URL, axiosClient } from '../../api/config';
import { CourseDto } from '../../api/dtos';
import { CreateCourseRequest, UpdateCourseRequest } from '../../api/requests';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { notifications } from '@mantine/notifications';
import { QueryKeys } from '../../api/query-keys';

const COURSES_API_URL = `${SERVER_URL}/Courses`;

export const useCreateCourse = () => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (request: CreateCourseRequest) => {
      await axiosClient.post(COURSES_API_URL, request);
    },
    onSuccess() {
      notifications.show({
        message: t('Course.Notification.CourseCreatedSuccessfully'),
        withBorder: true,
        withCloseButton: true,
        color: 'teal',
      });

      queryClient.invalidateQueries([QueryKeys.GetCourses]);
    },
    onError: () => {
      notifications.show({
        message: t('Course.Notification.FailedToCreateCourse'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });
    },
  });
};

export const useGetCourse = (courseId: number | null) => {
  const { t } = useTranslation();

  return useQuery([QueryKeys.GetCourse, courseId], async () => {
    if (!courseId) {
      return null;
    }

    try {
      const response = await axiosClient.get<CourseDto>(`${COURSES_API_URL}/${courseId}`);
      return response.data;
    } catch {
      notifications.show({
        message: t('Course.Notification.FailedToGetCourse'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });

      return null;
    }
  });
};

export const useGetCourses = () => {
  const { t } = useTranslation();

  return useQuery([QueryKeys.GetCourses], {
    queryFn: async () => {
      try {
        const response = await axiosClient.get<CourseDto[]>(COURSES_API_URL);
        return response.data;
      } catch {
        notifications.show({
          message: t('Course.Notification.FailedToGetCourses'),
          withBorder: true,
          withCloseButton: true,
          color: 'red',
        });

        return [];
      }
    },
  });
};

export const useUpdateCourse = () => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async ({ courseId, request }: { courseId: number; request: UpdateCourseRequest }) => {
      await axiosClient.patch(`${COURSES_API_URL}/${courseId}`, request);
    },
    onSuccess(_, { courseId }) {
      notifications.show({
        message: t('Course.Notification.CourseUpdatedSuccessfully'),
        withBorder: true,
        withCloseButton: true,
        color: 'teal',
      });

      queryClient.invalidateQueries([QueryKeys.GetCourse, courseId]);
      queryClient.invalidateQueries([QueryKeys.GetCourses]);
    },
    onError: () => {
      notifications.show({
        message: t('Course.Notification.FailedToUpdateCourse'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });
    },
  });
};

export const useDeleteCourse = () => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (courseId: number) => {
      await axiosClient.delete(`${COURSES_API_URL}/${courseId}`);
    },
    onSuccess() {
      notifications.show({
        message: t('Course.Notification.CourseDeletedSuccessfully'),
        withBorder: true,
        withCloseButton: true,
        color: 'teal',
      });

      queryClient.invalidateQueries([QueryKeys.GetCourses]);
    },
    onError: () => {
      notifications.show({
        message: t('Course.Notification.FailedToDeleteCourse'),
        withBorder: true,
        withCloseButton: true,
        color: 'red',
      });
    },
  });
};
