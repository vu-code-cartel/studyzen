import { ActionIcon, AppShell, Box, Burger, Group, Text, useMantineColorScheme, useMantineTheme } from '@mantine/core';
import { useAppStore } from '../hooks/useAppStore';
import { AppNavLink } from './AppFrameNavBarLink';
import { AppRoutes } from '../common/app-routes';
import { useTranslation } from 'react-i18next';
import { IconMoonStars, IconSun } from '@tabler/icons-react';
import { Link, Outlet } from 'react-router-dom';
import { useDisclosure } from '@mantine/hooks';

export const AppFrame = () => {
  const [isOpen, { toggle, close }] = useDisclosure();
  const activeCategory = useAppStore((state) => state.pageCategory);
  const { t } = useTranslation();
  const theme = useMantineTheme();
  const colorScheme = useMantineColorScheme();

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

      <AppShell.Navbar
        bg={colorScheme.colorScheme == 'dark' ? theme.colors.dark[8] : undefined}
        style={{ height: '100%' }}
      >
        <AppShell.Section p='md'>
          <Group justify='space-between'>
            <Text fw={600} component={Link} to={AppRoutes.Home}>
              {t('Common.AppName')}
            </Text>
            <ActionIcon variant='default' onClick={colorScheme.toggleColorScheme} title='Toggle color scheme'>
              {colorScheme.colorScheme === 'dark' ? <IconSun size='1rem' /> : <IconMoonStars size='1rem' />}
            </ActionIcon>
          </Group>
        </AppShell.Section>

        <AppShell.Section grow>
          <AppNavLink
            label={t('NavBar.Courses')}
            to={AppRoutes.Courses}
            isActive={activeCategory == 'courses'}
            onClick={close}
          />
          <AppNavLink
            label={t('NavBar.Flashcards')}
            to={'#'}
            isActive={activeCategory == 'flashcards'}
            onClick={close}
          />
        </AppShell.Section>
      </AppShell.Navbar>

      <AppShell.Main>
        <Box py='md'>
          <Outlet />
        </Box>
      </AppShell.Main>
    </AppShell>
  );
};
