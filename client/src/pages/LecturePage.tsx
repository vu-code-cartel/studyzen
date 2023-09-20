import { Container } from '@mantine/core';
import { PageHeader } from '../components/PageHeader';
import { useParams } from 'react-router-dom';
import { AppBreadcrumbs } from '../components/AppBreadcrumbs';
import { AppRoutes, getCourseRouteBySlug } from '../common/app-routes';

export const LecturePage = () => {
  const { courseIdWithSlug, lectureIdWithSlug } = useParams();

  if (!courseIdWithSlug || !lectureIdWithSlug) {
    return <div>TODO: not found</div>;
  }

  return (
    <Container>
      <PageHeader>
        <AppBreadcrumbs
          items={[
            { title: 'Courses', to: AppRoutes.Courses },
            { title: courseIdWithSlug, to: getCourseRouteBySlug(courseIdWithSlug, 'lectures') },
            {
              title: lectureIdWithSlug,
              to: getCourseRouteBySlug(courseIdWithSlug, 'lectures'),
            },
          ]}
        />
      </PageHeader>
    </Container>
  );
};
