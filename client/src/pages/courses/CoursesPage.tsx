import { Button, Card, Text, Image, SimpleGrid } from '@mantine/core';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';
import { AppRoutes, getCourseRoute } from '../../common/app-routes';
import { PageHeader } from '../../components/PageHeader';
import { AppBreadcrumbs } from '../../components/AppBreadcrumbs';
import { useDocumentTitle } from '@mantine/hooks';
import { usePageCategory } from '../../hooks/usePageCategory';
import { useButtonVariant } from '../../hooks/useButtonVariant';
import { useGetCourses } from '../../hooks/api/useCoursesApi';
import { CenteredLoader } from '../../components/CenteredLoader';
import { PageContainer } from '../../components/PageContainer';
import { useAppStore } from '../../hooks/useAppStore';

export const CoursesPage = () => {
  const { t } = useTranslation();
  const buttonVariant = useButtonVariant();
  const { data: courses, isLoading } = useGetCourses();
  useDocumentTitle(t('Course.DocumentTitle.Courses'));
  usePageCategory('courses');
  const colorScheme = useAppStore((state) => state.colorScheme);
  const userRole = useAppStore((state) => state.user?.role);

  if (!courses || isLoading) {
    return <CenteredLoader />;
  }

  return (
    <PageContainer>
      <PageHeader>
        <AppBreadcrumbs items={[{ title: t('Course.Title.Courses'), to: AppRoutes.Courses }]} />
        {userRole === 'Lecturer' && (
          <Button component={Link} to={AppRoutes.NewCourse} variant={buttonVariant}>
            {t('Course.Action.NewCourse')}
          </Button>
        )}
      </PageHeader>

      <SimpleGrid cols={{ base: 1, sm: 2, md: 3 }}>
        {courses.map((course) => (
          <Card withBorder component={Link} to={getCourseRoute(course.id, course.name)} key={course.id} shadow='sm'>
            <Card.Section>
              <Image
                height={160}
                fallbackSrc={
                  colorScheme == 'light'
                    ? `https://placehold.co/600x400/cccccc/232323?text=${course.name}`
                    : `https://placehold.co/600x400/232323/cccccc?text=${course.name}`
                }
              />
            </Card.Section>
            <Text fw={600} mt='md'>
              {course.name}
            </Text>
          </Card>
        ))}
      </SimpleGrid>
    </PageContainer>
  );
};
