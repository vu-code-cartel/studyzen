export type UserActionStamp = {
  user: string;
  timestamp: Date;
};

export enum Color {
  Default = 'Default',
  Blue = 'Blue',
  Green = 'Green',
  Red = 'Red',
  Purple = 'Purple',
  Yellow = 'Yellow',
}

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

export type FlashcardSetDto = {
  id: number;
  name: string;
  color: Color;
};

export type FlashcardDto = {
  id: number;
  front: string;
  back: string;
};
