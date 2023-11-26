import axios from 'axios';
import { notifications } from '@mantine/notifications';

// Change the URL according to your machine
export const SERVER_URL = 'http://localhost:5234';

export const axiosClient = axios.create({ withCredentials: true });
axiosClient.defaults.headers.post['Content-Type'] = 'application/json';
axiosClient.defaults.headers.put['Content-Type'] = 'application/json';
axiosClient.defaults.headers.patch['Content-Type'] = 'application/json';

axiosClient.interceptors.response.use(
    response => response,
    async error => {
        const originalRequest = error.config;
        if (error.response.status === 401 && !originalRequest._retry) {
            const wwwAuthenticateHeader = error.response.headers['www-authenticate'];
            if (wwwAuthenticateHeader && wwwAuthenticateHeader.includes('Bearer error="invalid_token"')) {
                originalRequest._retry = true;
                try {
                    await axiosClient.post(`${SERVER_URL}/Account/refresh-tokens`);
                    return axiosClient(originalRequest);
                } catch (refreshError) {
                    notifications.show({
                        message: 'Session expired. Please log in again.',
                        withBorder: true,
                        withCloseButton: true,
                        color: 'red',
                    });
                    return Promise.reject(refreshError);
                }
            }
        }
        return Promise.reject(error);
    }
);