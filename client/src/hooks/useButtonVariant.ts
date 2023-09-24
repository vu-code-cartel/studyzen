import { ButtonVariant } from '@mantine/core';
import { useAppStore } from './useAppStore';

export const useButtonVariant = (): ButtonVariant => {
  const colorScheme = useAppStore((state) => state.colorScheme);

  return colorScheme == 'light' ? 'filled' : 'light';
};
