import { Color, Role } from './dtos';

export type CreateCourseRequest = {
  name: string;
  description: string;
};

export type UpdateCourseRequest = {
  name: string | null;
  description: string | null;
};

export type CreateLectureRequest = {
  courseId: number;
  name: string;
  content: string;
};

export type UpdateLectureRequest = {
  name: string | null;
  content: string | null;
};

export type CreateFlashcardSetRequest = {
  lectureId?: number;
  name: string;
  color: Color;
};

export type UpdateFlashcardSetDto = {
  name?: string;
  color?: Color;
};

export type CreateFlashcardRequest = {
  flashcardSetId: number;
  front: string;
  back: string;
};

export type FlashcardsImportDto = {
  flashcardSetId: number;
  file: File;
};

export type UpdateFlashcardDto = {
  front?: string;
  back?: string;
};

export type JoinQuizGameDto = {
  gamePin: string;
  username: string;
  connectionId: string;
};

export type CreateQuizDto = {
  title: string;
};

export type CreateQuizQuestionDto = {
  question: string;
  correctAnswers: string[];
  incorrectAnswers: string[];
  timeLimitInSeconds: number;
};

export type LoginRequest = {
  email: string;
  password: string;
}

export type RegisterRequest = {
  username: string;
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  role: Role;
}