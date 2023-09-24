import { ColorSchemeScript, MantineProvider } from '@mantine/core';
import { Notifications } from '@mantine/notifications';

interface StylesProviderProps {
  children: React.ReactNode;
}

export const StylesProvider = (props: StylesProviderProps) => {
  return (
    <>
      <ColorSchemeScript defaultColorScheme='auto' />
      <MantineProvider defaultColorScheme='auto'>
        <Notifications />
        {props.children}
      </MantineProvider>
    </>
  );
};
