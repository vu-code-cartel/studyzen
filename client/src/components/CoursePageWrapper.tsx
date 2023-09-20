import { Container, Tabs, useMantineColorScheme } from '@mantine/core';
import { PageHeader } from './PageHeader';
import { AppBreadcrumbs } from './AppBreadcrumbs';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { AppRoutes, getCourseRouteBySlug } from '../common/app-routes';
import { CourseTab } from '../common/types';

interface CoursePageWrapperProps {
  tab: CourseTab;
  courseName: string;
  courseIdWithSlug: string;
  children: React.ReactNode;
}

export const CoursePageWrapper = (props: CoursePageWrapperProps) => {
  const navigate = useNavigate();
  const { colorScheme } = useMantineColorScheme();
  const { t } = useTranslation();

  return (
    <Container>
      <PageHeader>
        <AppBreadcrumbs
          items={[
            { title: 'Courses', to: AppRoutes.Courses },
            { title: props.courseName, to: getCourseRouteBySlug(props.courseIdWithSlug, 'lectures') },
          ]}
        />
      </PageHeader>

      <Tabs
        defaultValue={props.tab}
        mb='sm'
        color={colorScheme == 'dark' ? 'pink' : 'red'}
        onChange={(value) => {
          navigate(getCourseRouteBySlug(props.courseIdWithSlug, value as CourseTab));
        }}
      >
        <Tabs.List>
          <Tabs.Tab value='lectures'>{t('CoursePage.Tabs.Lectures')}</Tabs.Tab>
          <Tabs.Tab value='about'>{t('CoursePage.Tabs.About')}</Tabs.Tab>
        </Tabs.List>
      </Tabs>

      {props.children}
    </Container>
  );
};
