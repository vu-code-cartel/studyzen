import { QueryProvider } from './QueryProvider';
import { RouterProvider } from './RouterProvider';
import { StylesProvider } from './StylesProvider';

export const Providers = () => {
  return (
    <QueryProvider>
      <StylesProvider>
        <RouterProvider />
      </StylesProvider>
    </QueryProvider>
  );
};
