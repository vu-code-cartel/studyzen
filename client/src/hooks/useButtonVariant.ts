import { ButtonVariant, useMantineColorScheme } from '@mantine/core';

export const useButtonVariant = (): ButtonVariant => {
  const { colorScheme } = useMantineColorScheme();

  return colorScheme == 'dark' ? 'light' : 'filled';
};
