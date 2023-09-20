import { useEffect } from 'react';
import { useAppStore } from './useAppStore';
import { PageCategory } from '../common/types';

export const usePageCategory = (category: PageCategory) => {
  const setPageCategory = useAppStore((state) => state.setPageCategory);

  useEffect(() => {
    setPageCategory(category);
  }, [category, setPageCategory]);
};
