import { Button, Card, Text, Image, SimpleGrid, Container } from '@mantine/core';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';
import { AppRoutes, getCourseRoute } from '../common/app-routes';
import { PageHeader } from '../components/PageHeader';
import { CourseDto } from '../api/dtos';
import { AppBreadcrumbs } from '../components/AppBreadcrumbs';
import { useDocumentTitle } from '@mantine/hooks';
import { usePageCategory } from '../hooks/usePageCategory';
import { useButtonVariant } from '../hooks/useButtonVariant';

const courses: CourseDto[] = [
  {
    id: 1,
    name: 'Software Engineering I',
    description: 'just a test',
  },
  {
    id: 2,
    name: 'Database / Management',
    description: 'yep',
  },
  {
    id: 3,
    name: 'Mathematical Logic',
    description: 'dan',
  },
  {
    id: 4,
    name: 'Functional Programming',
    description: 'dan',
  },
];

export const CoursesPage = () => {
  const { t } = useTranslation();
  const buttonVariant = useButtonVariant();

  useDocumentTitle(t('CoursesPage.DocumentTitle'));
  usePageCategory('courses');

  return (
    <Container>
      <PageHeader>
        <AppBreadcrumbs items={[{ title: t('CoursesPage.Title'), to: AppRoutes.Courses }]} />
        <Button component={Link} to={AppRoutes.NewCourse} color='teal' variant={buttonVariant}>
          {t('CoursesPage.NewCourse')}
        </Button>
      </PageHeader>

      <SimpleGrid cols={{ base: 1, sm: 2, md: 3 }}>
        {courses.map((course) => (
          <Card
            padding='lg'
            withBorder
            component={Link}
            to={getCourseRoute(course.id, course.name, 'lectures')}
            key={course.id}
            shadow='sm'
          >
            <Card.Section>
              <Image height={160} fallbackSrc='https://placehold.co/600x400/232323/cccccc?text=Placeholder' />
            </Card.Section>
            <Text fw={600} mt='md'>
              {course.name}
            </Text>
          </Card>
        ))}
      </SimpleGrid>
    </Container>
  );
};
