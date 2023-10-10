import { UserActionStamp } from '../api/dtos';

export const getIdFromSlug = (idWithSlug?: string) => {
  if (!idWithSlug) {
    return null;
  } else {
    const num = Number(idWithSlug.split('-')[0]);

    if (isNaN(num)) {
      return null;
    } else {
      return num;
    }
  }
};

export const formatUserActionStamp = (stamp: UserActionStamp): UserActionStamp => ({
  ...stamp,
  timestamp: new Date(`${stamp.timestamp}`),
});
