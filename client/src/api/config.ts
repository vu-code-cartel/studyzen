import axios, { AxiosError, AxiosRequestConfig } from 'axios';
import { refreshToken } from '../hooks/api/useAccountsApi';
import { useAppStore } from '../hooks/useAppStore';

// Change the URL according to your machine
export const SERVER_URL = 'http://localhost:5234';

export const axiosClient = axios.create();
axiosClient.defaults.headers.post['Content-Type'] = 'application/json';
axiosClient.defaults.headers.put['Content-Type'] = 'application/json';
axiosClient.defaults.headers.patch['Content-Type'] = 'application/json';

// axiosClient.interceptors.response.use(
//     response => response,
//     async (error: AxiosError) => {
//         // Ensure that error.config is defined
//         if (!error.config) {
//             return Promise.reject(error);
//         }

//         const originalRequest = error.config;
//         const status = error.response?.status;
//         const wwwAuthenticateHeader = error.response?.headers['www-authenticate'];

//         if (status === 401 && wwwAuthenticateHeader && wwwAuthenticateHeader.includes("expired")) {
//             try {
//                 await refreshToken(); // Attempt to refresh the token
//                 return axiosClient(originalRequest); // Retry the original request
//             } catch (refreshError) {
//                 // If token refresh fails, set login status to false
//                 const setIsLoggedIn = useAppStore.getState().setIsLoggedIn;
//                 setIsLoggedIn(false);
//                 return Promise.reject(refreshError);
//             }
//         }

//         return Promise.reject(error);
//     }
// );
