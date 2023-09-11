import ChakraProvider from './ChakraProvider';
import { QueryProvider } from './QueryProvider';
import { RouterProvider } from './RouterProvider';

export const Providers = () => {
  return (
    <QueryProvider>
      <ChakraProvider>
        <RouterProvider />
      </ChakraProvider>
    </QueryProvider>
  );
};
