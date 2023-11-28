import { AppInitializer } from '../components/AppInitializer';
import { QueryProvider } from './QueryProvider';
import { RouterProvider } from './RouterProvider';
import { StylesProvider } from './StylesProvider';

export const Providers = () => {
  return (
    <QueryProvider>
      <AppInitializer>
        <StylesProvider>
          <RouterProvider />
        </StylesProvider>
      </AppInitializer>
    </QueryProvider>
  );
};
