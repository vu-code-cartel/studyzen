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
        path: '*',
        element: <NotFound />,
      },
    ],
  },
]);

export const RouterProvider = () => {
  return <Provider router={router} />;
};
