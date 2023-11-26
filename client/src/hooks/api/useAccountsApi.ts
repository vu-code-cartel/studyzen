import { useAppStore } from '../../hooks/useAppStore';
import { SERVER_URL, axiosClient } from '../../api/config';
import { LoginRequest, RegisterRequest } from '../../api/requests';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { notifications } from '@mantine/notifications';
import { useTranslation } from 'react-i18next';
import { QueryKeys } from '../../api/query-keys';
import axios from 'axios';

const ACCOUNTS_API_URL = `${SERVER_URL}/Account`;

export const useLogin = (onSuccessCallback?: () => void) => {
    const { t } = useTranslation();
    const queryClient = useQueryClient();
    const fetchUserInfo = useFetchUserInfo();

    return useMutation({
        mutationFn: async (request: LoginRequest) => {
            await axiosClient.post(`${ACCOUNTS_API_URL}/login`, request, { withCredentials: true });
        },
        onSuccess: () => {
            fetchUserInfo();
            notifications.show({
                message: t('Authentication.Notification.LoginSuccessful'),
                withBorder: true,
                withCloseButton: true,
                color: 'teal',
            });
            queryClient.invalidateQueries([QueryKeys.Login]);

            if (onSuccessCallback) onSuccessCallback();
        },
        onError: (error: unknown) => {
            let message = t('Authentication.Error.LoginFailed');

            if (axios.isAxiosError(error)) {
                const serverResponse = error.response?.data;

                if (error.response?.status === 401 && serverResponse?.detail === "Invalid email or password") {
                    message = t('Authentication.Error.InvalidCredentials');
                }
            }

            notifications.show({
                message: message,
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
    const setUser = useAppStore((state) => state.setUser);

    const mutation = useMutation({
        mutationFn: async () => {
            await axiosClient.post(`${ACCOUNTS_API_URL}/logout`, null, { withCredentials: true });
        },
        onSuccess() {
            setUser('', '', '');
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
                message: t('Authentication.Error.LogoutFailed'),
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

export const useRegister = (onSuccessCallback?: () => void) => {
    const { t } = useTranslation();
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (request: RegisterRequest) => {
            await axiosClient.post(`${ACCOUNTS_API_URL}/register`, request, { withCredentials: true });
        },
        onSuccess() {
            queryClient.removeQueries();

            notifications.show({
                message: t('Authentication.Notification.RegisterSuccessful'),
                withBorder: true,
                withCloseButton: true,
                color: 'green',
            });

            if (onSuccessCallback) onSuccessCallback();
        },
        onError: (error: unknown) => {
            let message = t('Authentication.Error.RegisterFailed');

            if (axios.isAxiosError(error) && error.response) {
                const serverResponse = error.response.data;

                if (error.response.status === 422 && serverResponse?.detail.includes("already exists")) {
                    message = t('Authentication.Error.UserExists', { email: serverResponse.detail.match(/[^ ]+@[^ ]+/)[0] });
                }
            }

            notifications.show({
                message: message,
                withBorder: true,
                withCloseButton: true,
                color: 'red',
            });
        },
    });
}

export const useFetchUserInfo = () => {
    const setUser = useAppStore((state) => state.setUser);
    const setIsLoggedIn = useAppStore((state) => state.setIsLoggedIn);

    const fetchUserInfo = async () => {
        try {
            const response = await axiosClient.get(`${ACCOUNTS_API_URL}/user`);
            if (response.status === 200) {
                const { id, username, role } = response.data;
                setUser(id, username, role);
                setIsLoggedIn(true);
            }
        } catch (error) {
            if (axios.isAxiosError(error) && error.response?.status === 401) {
                setUser('', '', '');
            }
            setIsLoggedIn(false);
            throw error;
        }
    };

    return fetchUserInfo;
};