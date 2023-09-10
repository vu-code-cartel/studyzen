import { ChakraProvider as Provider, extendTheme } from '@chakra-ui/react';

const theme = extendTheme({
  fonts: {
    heading: `"Inter", sans-serif`,
    body: `"Inter", sans-serif`,
  },
});

interface ChakraProviderProps {
  children: React.ReactNode;
}

const ChakraProvider = (props: ChakraProviderProps) => {
  return <Provider theme={theme}>{props.children}</Provider>;
};

export default ChakraProvider;
