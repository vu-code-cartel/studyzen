import { rgba, useMantineTheme } from '@mantine/core';
import { Color } from '../api/dtos';
import { useAppStore } from './useAppStore';

export const useColor = (color: Color) => {
  const theme = useMantineTheme();
  const colorScheme = useAppStore((state) => state.colorScheme);

  const getBackgroundColor = (color: Color) => {
    switch (color) {
      case Color.Blue:
        return rgba(theme.colors.blue[4], 0.08);
      case Color.Green:
        return rgba(theme.colors.green[4], 0.08);
      case Color.Red:
        return rgba(theme.colors.red[4], 0.08);
      case Color.Purple:
        return rgba(theme.colors.pink[4], 0.08);
      case Color.Yellow:
        return rgba(theme.colors.yellow[4], 0.08);
      default:
        return rgba(theme.colors.dark[colorScheme == 'light' ? 2 : 1], 0.08);
    }
  };

  const getTextColor = (color: Color) => {
    switch (color) {
      case Color.Blue:
        return theme.colors.blue[colorScheme == 'light' ? 7 : 4];
      case Color.Green:
        return theme.colors.green[colorScheme == 'light' ? 7 : 4];
      case Color.Red:
        return theme.colors.red[colorScheme == 'light' ? 7 : 5];
      case Color.Purple:
        return theme.colors.pink[colorScheme == 'light' ? 7 : 5];
      case Color.Yellow:
        return theme.colors.yellow[colorScheme == 'light' ? 7 : 4];
      default:
        return theme.colors.dark[colorScheme == 'light' ? 6 : 1];
    }
  };

  const getBorderColor = (color: Color) => {
    switch (color) {
      case Color.Blue:
        return theme.colors.blue[6];
      case Color.Green:
        return theme.colors.green[6];
      case Color.Red:
        return theme.colors.red[6];
      case Color.Purple:
        return theme.colors.pink[6];
      case Color.Yellow:
        return theme.colors.yellow[6];
      default:
        return colorScheme == 'light' ? theme.colors.dark[0] : theme.colors.dark[4];
    }
  };

  return [getTextColor(color), getBackgroundColor(color), getBorderColor(color)];
};
