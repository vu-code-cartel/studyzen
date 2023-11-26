import { useAppStore } from '../../hooks/useAppStore';
import { SERVER_URL, axiosClient } from '../../api/config';
import { LoginRequest } from '../../api/requests';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { notifications } from '@mantine/notifications';
import { useTranslation } from 'react-i18next';
import { QueryKeys } from '../../api/query-keys';

const ACCOUNTS_API_URL = `${SERVER_URL}/Account`;

export const useCreateCourse = () => {
    const { t } = useTranslation();
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (request: LoginRequest) => {
            await axiosClient.post(`${SERVER_URL}/Account/login`, request);
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

export const useLogout = () => {
    const { t } = useTranslation();
    const queryClient = useQueryClient();
    const setIsLoggedIn = useAppStore((state) => state.setIsLoggedIn);

    const mutation = useMutation({
        mutationFn: async () => {
            await axiosClient.post(`${ACCOUNTS_API_URL}/logout`, null, { withCredentials: true });
        },
        onSuccess() {
            setIsLoggedIn(false);
            queryClient.removeQueries();

            notifications.show({
                message: t('Authentication.Notification.LogoutSuccessful'),
                withBorder: true,
                withCloseButton: true,
                color: 'green',
            });
        },
        onError() {
            notifications.show({
                message: t('Authentication.Notification.LogoutFailed'),
                withBorder: true,
                withCloseButton: true,
                color: 'red',
            });
        },
    });

    return {
        logout: mutation.mutate,
        isLoading: mutation.isLoading,
    };
};

export const refreshToken = async () => {
    const response = await axiosClient.post(`${SERVER_URL}/Account/refresh-token`, {}, { withCredentials: true });
    return response.data;
};