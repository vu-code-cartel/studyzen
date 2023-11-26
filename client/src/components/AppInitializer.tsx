import { ReactNode, useEffect } from 'react';
import { useFetchUserInfo } from '../hooks/api/useAccountsApi';

interface AppInitializerProps {
    children: ReactNode;
}

const AppInitializer: React.FC<AppInitializerProps> = ({ children }) => {
    const fetchUserInfo = useFetchUserInfo();

    useEffect(() => {
        fetchUserInfo();
    }, [fetchUserInfo]);

    return <>{children}</>;
};

export default AppInitializer;