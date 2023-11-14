import { ActionIcon, AppShell, Box, Burger, Group, Text, useMantineColorScheme, useMantineTheme } from '@mantine/core';
import { useAppStore } from '../hooks/useAppStore';
import { AppNavLink } from './AppFrameNavBarLink';
import { AppRoutes } from '../common/app-routes';
import { useTranslation } from 'react-i18next';
import { IconMoonStars, IconSun } from '@tabler/icons-react';
import { Link, Outlet } from 'react-router-dom';
import { useColorScheme, useDisclosure, useMediaQuery } from '@mantine/hooks';
import { useEffect } from 'react';

export const AppFrame = () => {
  const [isOpen, { toggle, close }] = useDisclosure();
  const activeCategory = useAppStore((state) => state.pageCategory);
  const setIsMobile = useAppStore((state) => state.setIsMobile);
  const setColorScheme = useAppStore((state) => state.setColorScheme);
  const colorScheme = useAppStore((state) => state.colorScheme);
  const { t } = useTranslation();
  const theme = useMantineTheme();
  const isMobile = useMediaQuery(`(max-width: ${theme.breakpoints.sm})`);
  const { colorScheme: mantineColorScheme, toggleColorScheme } = useMantineColorScheme();
  const mediaColorScheme = useColorScheme();

  useEffect(() => {
    if (mantineColorScheme != 'auto') {
      setColorScheme(mantineColorScheme);
    } else {
      setColorScheme(mediaColorScheme);
    }
  }, [mantineColorScheme, mediaColorScheme, setColorScheme]);

  useEffect(() => {
    setIsMobile(isMobile);
  }, [setIsMobile, isMobile]);

  return (
    <AppShell
      transitionTimingFunction='ease'
      transitionDuration={200}
      navbar={{ width: 300, breakpoint: 'sm', collapsed: { mobile: !isOpen } }}
      header={{ height: { base: 48, sm: 0 } }}
    >
      <AppShell.Header px='md' hiddenFrom='sm'>
        <div style={{ display: 'flex', alignItems: 'center', height: '100%' }}>
          <Burger opened={isOpen} onClick={toggle} size='sm' mr='xl' p={0} />
        </div>
      </AppShell.Header>

      <AppShell.Navbar bg={colorScheme == 'dark' ? theme.colors.dark[8] : undefined} style={{ height: '100%' }}>
        <AppShell.Section p='md'>
          <Group justify='space-between'>
            <Text fw={600} component={Link} to={AppRoutes.Home}>
              {t('App.Title')}
            </Text>
            <ActionIcon variant='default' onClick={toggleColorScheme} title={t('App.Nav.ToggleColorScheme')}>
              {colorScheme === 'dark' ? <IconSun size='1rem' /> : <IconMoonStars size='1rem' />}
            </ActionIcon>
          </Group>
        </AppShell.Section>

        <AppShell.Section grow>
          <AppNavLink
            label={t('App.Nav.Courses')}
            to={AppRoutes.Courses}
            isActive={activeCategory == 'courses'}
            onClick={close}
          />
          <AppNavLink
            label={t('App.Nav.Flashcards')}
            to={AppRoutes.FlashcardSets}
            isActive={activeCategory == 'flashcards'}
            onClick={close}
          />
          <AppNavLink
            label={t('App.Nav.Quizzes')}
            to={AppRoutes.Quizzes}
            isActive={activeCategory == 'quizzes'}
            onClick={close}
          />
        </AppShell.Section>
      </AppShell.Navbar>

      <AppShell.Main h='100vh' bg={colorScheme === 'dark' ? theme.colors.dark[7] : theme.colors.gray[0]}>
        <Box py='md' h='100%'>
          <Outlet />
        </Box>
      </AppShell.Main>
    </AppShell>
  );
};
