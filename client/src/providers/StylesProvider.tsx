import { ColorSchemeScript, MantineProvider } from '@mantine/core';

interface StylesProviderProps {
  children: React.ReactNode;
}

export const StylesProvider = (props: StylesProviderProps) => {
  return (
    <>
      <ColorSchemeScript defaultColorScheme='auto' />
      <MantineProvider defaultColorScheme='auto'>{props.children}</MantineProvider>
    </>
  );
};
