import { useTranslation } from 'react-i18next';
import { PageHeader } from '../../components/PageHeader';
import { AppBreadcrumbs } from '../../components/AppBreadcrumbs';
import { AppRoutes } from '../../common/app-routes';
import { usePageCategory } from '../../hooks/usePageCategory';
import { useDocumentTitle } from '@mantine/hooks';
import { useCreateCourse } from '../../hooks/api/useCoursesApi';
import { useNavigate } from 'react-router-dom';
import { CreateCourseRequest } from '../../api/requests';
import { PageContainer } from '../../components/PageContainer';
import { CourseForm } from '../../components/CourseForm';

export const NewCoursePage = () => {
  const { t } = useTranslation();
  useDocumentTitle(t('Course.DocumentTitle.NewCourse'));
  usePageCategory('courses');

  return (
    <PageContainer>
      <PageHeader>
        <AppBreadcrumbs
          items={[
            { title: t('Course.Title.Courses'), to: AppRoutes.Courses },
            { title: t('Course.Title.NewCourse'), to: AppRoutes.NewCourse },
          ]}
        />
      </PageHeader>

      <NewCourseForm />
    </PageContainer>
  );
};

const NewCourseForm = () => {
  const navigate = useNavigate();
  const createCourseMutation = useCreateCourse();

  const onCourseCreateClick = async (request: CreateCourseRequest) => {
    await createCourseMutation.mutateAsync(request);
    navigate(AppRoutes.Courses);
  };

  return <CourseForm onSubmit={onCourseCreateClick} />;
};
