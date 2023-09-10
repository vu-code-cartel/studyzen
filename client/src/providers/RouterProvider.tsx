import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import MainPage from '../pages/MainPage';
import NotFoundPage from '../pages/NotFoundPage';

export class AppRoutes {
  public static readonly MainPage = '/';
}

const RouterProvider = () => {
  return (
    <Router>
      <Routes>
        <Route path={AppRoutes.MainPage} element={<MainPage />} />
        <Route path='*' element={<NotFoundPage />} />
      </Routes>
    </Router>
  );
};

export default RouterProvider;
