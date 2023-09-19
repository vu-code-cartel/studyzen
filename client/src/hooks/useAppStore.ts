import { create } from 'zustand';
import { PageCategory } from '../common/types';

interface AppState {
  pageCategory: PageCategory;
  setPageCategory: (category: PageCategory) => void;
}

export const useAppStore = create<AppState>((set) => ({
  pageCategory: 'unknown',
  setPageCategory: (category) => set(() => ({ pageCategory: category, isAppFrameVisible: true })),
}));
