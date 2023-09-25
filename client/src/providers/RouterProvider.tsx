import { RouterProvider as Provider, createBrowserRouter } from 'react-router-dom';
import { HomePage } from '../pages/HomePage';
import { AppRoutes } from '../common/app-routes';
import { CoursesPage } from '../pages/CoursesPage';
import { NewCoursePage } from '../pages/NewCoursePage';
import { LecturePage } from '../pages/LecturePage';
import { NewLecturePage } from '../pages/NewLecturePage';
import { CoursePage } from '../pages/CoursePage';
import { AppFrame } from '../components/AppFrame';
import { NotFound } from '../components/NotFound';

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
        path: '*',
        element: <NotFound />,
      },
    ],
  },
]);

export const RouterProvider = () => {
  return <Provider router={router} />;
};
