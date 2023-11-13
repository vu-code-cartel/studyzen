import { RouterProvider as Provider, createBrowserRouter } from 'react-router-dom';
import { HomePage } from '../pages/HomePage';
import { AppRoutes } from '../common/app-routes';
import { CoursesPage } from '../pages/courses/CoursesPage';
import { NewCoursePage } from '../pages/courses/NewCoursePage';
import { LecturePage } from '../pages/lectures/LecturePage';
import { NewLecturePage } from '../pages/lectures/NewLecturePage';
import { CoursePage } from '../pages/courses/CoursePage';
import { AppFrame } from '../components/AppFrame';
import { NotFound } from '../components/NotFound';
import { FlashcardSetsPage } from '../pages/flashcards/FlashcardSetsPage';
import { NewFlashcardSetPage } from '../pages/flashcards/NewFlashcardSetPage';
import { FlashcardSetPage } from '../pages/flashcards/FlashcardSetPage';
import { QuizzesPage } from '../pages/quiz/QuizzesPage';
import { QuizPage } from '../pages/quiz/QuizPage';
import { QuizRoomPage } from '../pages/quiz/QuizRoomPage';
import { NewQuizPage } from '../pages/quiz/NewQuizPage';

const router = createBrowserRouter([
  {
    element: <AppFrame />,
    children: [
      {
        path: AppRoutes.Home,
        element: <HomePage />,
      },
      {
        path: AppRoutes.Courses,
        element: <CoursesPage />,
      },
      {
        path: AppRoutes.Course,
        element: <CoursePage />,
      },
      {
        path: AppRoutes.NewCourse,
        element: <NewCoursePage />,
      },
      {
        path: AppRoutes.Lecture,
        element: <LecturePage />,
      },
      {
        path: AppRoutes.NewLecture,
        element: <NewLecturePage />,
      },
      {
        path: AppRoutes.FlashcardSets,
        element: <FlashcardSetsPage />,
      },
      {
        path: AppRoutes.NewFlashcardSet,
        element: <NewFlashcardSetPage />,
      },
      {
        path: AppRoutes.FlashcardSet,
        element: <FlashcardSetPage />,
      },
      {
        path: AppRoutes.Quizzes,
        element: <QuizzesPage />,
      },
      {
        path: AppRoutes.NewQuiz,
        element: <NewQuizPage />,
      },
      {
        path: AppRoutes.Quiz,
        element: <QuizPage />,
      },
      {
        path: AppRoutes.QuizGameRoom,
        element: <QuizRoomPage />,
      },
      {
        path: '*',
        element: <NotFound />,
      },
    ],
  },
]);

export const RouterProvider = () => {
  return <Provider router={router} />;
};
