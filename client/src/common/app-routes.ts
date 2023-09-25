import urlSlug from 'url-slug';

export class AppRoutes {
  public static readonly Home = '/';
  public static readonly Courses = '/courses';
  public static readonly Course = `${this.Courses}/:courseIdWithSlug`;
  public static readonly NewCourse = `${this.Courses}/new`;
  public static readonly Lecture = `${this.Course}/:lectureIdWithSlug`;
  public static readonly NewLecture = `${this.Course}/new-lecture`;
}

export const getCourseRoute = (courseId: number, courseName: string) =>
  `${AppRoutes.Courses}/${courseId}-${urlSlug(courseName)}`;

export const getLectureRoute = (courseId: number, courseName: string, lectureId: number, lectureName: string) =>
  `${getCourseRoute(courseId, courseName)}/${lectureId}-${urlSlug(lectureName)}`;

export const getNewLectureRoute = (courseId: number, courseName: string) =>
  `${getCourseRoute(courseId, courseName)}/new-lecture`;
