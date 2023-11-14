import { ColorSchemeScript, MantineProvider } from '@mantine/core';
import { Notifications } from '@mantine/notifications';

interface StylesProviderProps {
  children: React.ReactNode;
}

export const StylesProvider = (props: StylesProviderProps) => {
  return (
    <>
      <ColorSchemeScript defaultColorScheme='auto' />
      <MantineProvider defaultColorScheme='auto' theme={{ primaryColor: 'indigo' }}>
        <Notifications position='top-center' limit={3} />
        {props.children}
      </MantineProvider>
    </>
  );
};
