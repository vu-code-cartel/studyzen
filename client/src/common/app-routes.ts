import urlSlug from 'url-slug';

export class AppRoutes {
  public static readonly Home = '/';
  public static readonly Courses = '/courses';
  public static readonly Course = `${this.Courses}/:courseIdWithSlug`;
  public static readonly NewCourse = `${this.Courses}/new`;
  public static readonly Lecture = `${this.Course}/:lectureIdWithSlug`;
  public static readonly NewLecture = `${this.Course}/new-lecture`;
  public static readonly FlashcardSets = '/flashcards';
  public static readonly NewFlashcardSet = `${this.FlashcardSets}/new`;
  public static readonly FlashcardSet = `${this.FlashcardSets}/:flashcardSetIdWithSlug`;
  public static readonly Quizzes = '/quizzes';
  public static readonly NewQuiz = `${this.Quizzes}/new`;
  public static readonly Quiz = `${this.Quizzes}/:quizIdWithSlug`;
  public static readonly QuizGames = `${this.Quizzes}/games`;
  public static readonly QuizGameRoom = `${this.QuizGames}/:gamePin`;
}

export const getCourseRoute = (courseId: number, courseName: string) =>
  `${AppRoutes.Courses}/${courseId}-${urlSlug(courseName)}`;

export const getLectureRoute = (courseId: number, courseName: string, lectureId: number, lectureName: string) =>
  `${getCourseRoute(courseId, courseName)}/${lectureId}-${urlSlug(lectureName)}`;

export const getNewLectureRoute = (courseId: number, courseName: string) =>
  `${getCourseRoute(courseId, courseName)}/new-lecture`;

export const getFlashcardSetRoute = (flashcardSetId: number, flashcardSetName: string) =>
  `${AppRoutes.FlashcardSets}/${flashcardSetId}-${urlSlug(flashcardSetName)}`;

export const getQuizRoute = (quizId: number, quizTitle: string) =>
  `${AppRoutes.Quizzes}/${quizId}-${urlSlug(quizTitle)}`;

export const getQuizGameRoomRoute = (gamePin: string) => `${AppRoutes.QuizGames}/${gamePin}`;
