import { useQuery } from '@tanstack/react-query';
import axios from 'axios';
import { axiosClient } from '../api/config';
import { User, useAppStore } from '../hooks/useAppStore';
import { SERVER_URL } from '../api/config';
import { QueryKeys } from '../api/query-keys';

export const AppInitializer: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const setUser = useAppStore((state) => state.setUser);

  const fetchUserInfo = async () => {
    const response = await axiosClient.get(`${SERVER_URL}/account/user`);
    return response.data;
  };

  useQuery<User>([QueryKeys.GetUserInfo], fetchUserInfo, {
    onSuccess: (user) => {
      setUser(user);
    },
    onError: (error) => {
      if (axios.isAxiosError(error) && error.response?.status === 401) {
        setUser(null);
      }
    },
    retry: false,
  });

  return <>{children}</>;
};
