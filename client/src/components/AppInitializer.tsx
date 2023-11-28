import { useQuery } from '@tanstack/react-query';
import axios from 'axios';
import { axiosClient } from '../api/config';
import { useAppStore } from '../hooks/useAppStore';
import { SERVER_URL } from '../api/config';
import { QueryKeys } from '../api/query-keys';

export const AppInitializer: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const setUser = useAppStore((state) => state.setUser);
    const setIsLoggedIn = useAppStore((state) => state.setIsLoggedIn);

    const fetchUserInfo = async () => {
        const response = await axiosClient.get(`${SERVER_URL}/account/user`);
        return response.data;
    };

    useQuery([QueryKeys.GetUserInfo], fetchUserInfo, {
        onSuccess: (data) => {
            const { id, username, role } = data;
            setUser(id, username, role);
            setIsLoggedIn(true);
        },
        onError: (error) => {
            if (axios.isAxiosError(error) && error.response?.status === 401) {
                setUser('', '', '');
                setIsLoggedIn(false);
            }
        }
    });

    return <>{children}</>;
};