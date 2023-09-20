import urlSlug from 'url-slug';
import { CourseTab } from './types';

export class AppRoutes {
  public static readonly Home = '/';
  public static readonly Courses = '/courses';
  public static readonly Course = `${this.Courses}/:courseIdWithSlug`;
  public static readonly NewCourse = `${this.Courses}/new`;
  public static readonly CourseAbout = `${this.Course}/about`;
  public static readonly Lecture = `${this.Course}/:lectureIdWithSlug`;
  public static readonly NewLecture = `${this.Course}/new-lecture`;
}

const getBaseCourseRoute = (courseId: number, courseName: string) =>
  `${AppRoutes.Courses}/${courseId}-${urlSlug(courseName)}`;

export const getCourseRoute = (courseId: number, courseName: string, tab: CourseTab) =>
  `${getBaseCourseRoute(courseId, courseName)}/${tab == 'lectures' ? '' : tab}`;

export const getCourseRouteBySlug = (courseIdWithSlug: string, tab: CourseTab) =>
  `${AppRoutes.Courses}/${courseIdWithSlug}/${tab == 'lectures' ? '' : tab}`;

export const getLectureRoute = (courseId: number, courseName: string, lectureId: number, lectureName: string) =>
  `${getBaseCourseRoute(courseId, courseName)}/${lectureId}-${urlSlug(lectureName)}`;

export const getNewLectureRoute = (courseIdWithSlug: string) => `${AppRoutes.Courses}/${courseIdWithSlug}/new-lecture`;
