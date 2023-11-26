export enum Color {
  Default = 'Default',
  Blue = 'Blue',
  Green = 'Green',
  Red = 'Red',
  Purple = 'Purple',
  Yellow = 'Yellow',
}

export enum QuizGameState {
  Unknown = 'Unknown',
  NotStarted = 'NotStarted',
  InProgress = 'InProgress',
  Finished = 'Finished',
}

export type UserActionStamp = {
  user: string;
  timestamp: Date;
};

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

export type QuizDto = {
  id: number;
  title: string;
};

export type QuizGameDto = {
  pin: string;
  quiz: QuizDto;
  state: QuizGameState;
};

export type QuizQuestionDto = {
  id: number;
  question: string;
  choices: QuizAnswerDto[];
};

export type QuizAnswerDto = {
  id: number;
  answer: string;
  isCorrect: boolean;
};

export type QuizPlayerDto = {
  gamePin: string;
  username: string;
  accumulatedPoints: number;
};

export type QuizGameChoiceDto = {
  id: number;
  answer: string;
};

export type QuizGameQuestionDto = {
  question: string;
  choices: QuizGameChoiceDto[];
};