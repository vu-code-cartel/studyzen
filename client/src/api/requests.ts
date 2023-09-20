export type CreateCourseRequest = {
  name: string;
  description: string;
};

export type UpdateCourseRequest = {
  name?: string;
  description?: string;
};

export type CreateLectureRequest = {
  courseId: number;
  name: string;
  content: string;
};

export type UpdateLectureRequest = {
  name?: string;
  content?: string;
};
