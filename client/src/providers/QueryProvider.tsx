import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

interface QueryProviderProps {
  children: React.ReactNode;
}

export const QueryProvider = (props: QueryProviderProps) => {
  const client = new QueryClient({
    defaultOptions: {
      queries: {
        refetchOnWindowFocus: false,
        cacheTime: 5 * 60 * 1000,
        staleTime: 5 * 60 * 1000,
      },
    },
  });

  return <QueryClientProvider client={client}>{props.children}</QueryClientProvider>;
};
