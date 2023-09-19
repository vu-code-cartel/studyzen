import { useEffect, useState } from 'react';

export const useSlugId = (idWithSlug?: string) => {
  const [id, setId] = useState<number | undefined | null>(undefined);

  useEffect(() => {
    if (!idWithSlug) {
      setId(null);
    } else {
      const num = Number(idWithSlug.split('-')[0]);

      if (isNaN(num)) {
        setId(null);
      } else {
        setId(num);
      }
    }
  }, [idWithSlug]);

  return id;
};
