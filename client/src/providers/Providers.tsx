import AppInitializer from '../components/AppInitializer';
import { QueryProvider } from './QueryProvider';
import { RouterProvider } from './RouterProvider';
import { StylesProvider } from './StylesProvider';

export const Providers = () => {
  return (
    <QueryProvider>
      <StylesProvider>
        <AppInitializer>
          <RouterProvider />
        </AppInitializer>
      </StylesProvider>
    </QueryProvider>
  );
};
