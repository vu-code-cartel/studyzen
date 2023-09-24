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
