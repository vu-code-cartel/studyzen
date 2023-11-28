import { create } from 'zustand';
import { PageCategory } from '../common/types';

export type ColorScheme = 'light' | 'dark' | undefined;

interface User {
  id: string;
  username: string;
  role: string;
}

interface AppState {
  pageCategory: PageCategory;
  setPageCategory: (category: PageCategory) => void;
  isMobile: boolean | undefined;
  setIsMobile: (isMobile: boolean | undefined) => void;
  colorScheme: ColorScheme;
  setColorScheme: (scheme: ColorScheme) => void;
  user: User | null;
  setUser: (user: User | null) => void;
}

export const useAppStore = create<AppState>((set) => ({
  pageCategory: 'unknown',
  setPageCategory: (category) => set(() => ({ pageCategory: category })),
  isMobile: undefined,
  setIsMobile: (value) => set(() => ({ isMobile: value })),
  colorScheme: undefined,
  setColorScheme: (scheme) => set(() => ({ colorScheme: scheme })),
  user: null,
  setUser: (user) => set({ user }),
}));
