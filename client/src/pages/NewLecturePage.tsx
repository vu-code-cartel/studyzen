import { Button, Container, Fieldset, Stack, TextInput } from '@mantine/core';
import { useParams } from 'react-router-dom';
import { PageHeader } from '../components/PageHeader';
import { AppBreadcrumbs } from '../components/AppBreadcrumbs';
import { AppRoutes, getCourseRouteBySlug, getNewLectureRoute } from '../common/app-routes';
import { usePageCategory } from '../hooks/usePageCategory';
import { useButtonVariant } from '../hooks/useButtonVariant';

export const NewLecturePage = () => {
  const { courseIdWithSlug } = useParams();
  const buttonVariant = useButtonVariant();

  usePageCategory('courses');

  if (!courseIdWithSlug) {
    return <div>TODO: not found</div>;
  }

  return (
    <Container>
      <PageHeader>
        <AppBreadcrumbs
          items={[
            { title: 'Courses', to: AppRoutes.Courses },
            { title: courseIdWithSlug, to: getCourseRouteBySlug(courseIdWithSlug, 'lectures') },
            { title: 'New lecture', to: getNewLectureRoute(courseIdWithSlug) },
          ]}
        />
      </PageHeader>

      <form>
        <Fieldset>
          <Stack>
            <TextInput label='Name' />
            <Button type='submit' color='teal' variant={buttonVariant}>
              Create
            </Button>
          </Stack>
        </Fieldset>
      </form>
    </Container>
  );
};
