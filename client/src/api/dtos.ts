export type CourseDto = {
  id: number;
  name: string;
  description: string;
  createdBy: UserActionStamp;
  updatedBy: UserActionStamp;
};

export type LectureDto = {
  id: number;
  name: string;
  content: string;
  createdBy: UserActionStamp;
  updatedBy: UserActionStamp;
};

export type UserActionStamp = {
  user: string;
  timestamp: Date;
};
