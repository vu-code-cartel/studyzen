import { create } from 'zustand';
import { PageCategory } from '../common/types';

type ColorScheme = 'light' | 'dark' | undefined;

interface AppState {
  pageCategory: PageCategory;
  setPageCategory: (category: PageCategory) => void;
  isMobile: boolean | undefined;
  setIsMobile: (isMobile: boolean | undefined) => void;
  colorScheme: ColorScheme;
  setColorScheme: (scheme: ColorScheme) => void;
}

export const useAppStore = create<AppState>((set) => ({
  pageCategory: 'unknown',
  setPageCategory: (category) => set(() => ({ pageCategory: category })),
  isMobile: undefined,
  setIsMobile: (value) => set(() => ({ isMobile: value })),
  colorScheme: undefined,
  setColorScheme: (scheme) => set(() => ({ colorScheme: scheme })),
}));
