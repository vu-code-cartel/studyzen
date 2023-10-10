import { Button, Fieldset, Stack, TextInput } from '@mantine/core';
import { useNavigate, useParams } from 'react-router-dom';
import { PageHeader } from '../../components/PageHeader';
import { AppBreadcrumbs } from '../../components/AppBreadcrumbs';
import { AppRoutes, getCourseRoute, getNewLectureRoute } from '../../common/app-routes';
import { usePageCategory } from '../../hooks/usePageCategory';
import { useButtonVariant } from '../../hooks/useButtonVariant';
import { CreateLectureRequest } from '../../api/requests';
import { useTranslation } from 'react-i18next';
import { useCreateLecture } from '../../hooks/api/useLecturesApi';
import { CenteredLoader } from '../../components/CenteredLoader';
import { NotFound } from '../../components/NotFound';
import { useForm } from '@mantine/form';
import { useGetCourse } from '../../hooks/api/useCoursesApi';
import { getIdFromSlug } from '../../common/utils';
import { ControlledRichTextEditor } from '../../components/ControlledRichTextEditor';
import { useDocumentTitle } from '@mantine/hooks';
import { CourseDto } from '../../api/dtos';
import { PageContainer } from '../../components/PageContainer';

export const NewLecturePage = () => {
  const { courseIdWithSlug } = useParams();
  const { t } = useTranslation();
  const { data: course, isLoading: isCourseLoading } = useGetCourse(getIdFromSlug(courseIdWithSlug));
  usePageCategory('courses');
  useDocumentTitle(t('Lecture.DocumentTitle.NewLecture', { courseName: course?.name }));

  if (isCourseLoading) {
    return <CenteredLoader />;
  }

  if (!courseIdWithSlug || !course) {
    return <NotFound />;
  }

  return (
    <PageContainer>
      <PageHeader>
        <AppBreadcrumbs
          items={[
            { title: t('Course.Title.Courses'), to: AppRoutes.Courses },
            { title: course.name, to: getCourseRoute(course.id, course.name) },
            { title: t('Lecture.Title.NewLecture'), to: getNewLectureRoute(course.id, course.name) },
          ]}
        />
      </PageHeader>

      <NewLectureForm course={course} />
    </PageContainer>
  );
};

interface NewLectureFormProps {
  course: CourseDto;
}

const NewLectureForm = (props: NewLectureFormProps) => {
  const buttonVariant = useButtonVariant();
  const { t } = useTranslation();
  const navigate = useNavigate();
  const form = useForm<CreateLectureRequest>({
    initialValues: {
      courseId: 0,
      name: '',
      content: '',
    },
    validate: {
      name: (value) => (value ? null : t('Lecture.Validation.NameMustNotBeEmpty')),
      content: (value) => (value ? null : t('Lecture.Validation.ContentMustNotBeEmpty')),
    },
  });

  const createLectureMutation = useCreateLecture();

  const onCreateLectureClick = async (request: CreateLectureRequest) => {
    request.courseId = props.course.id;
    await createLectureMutation.mutateAsync(request);
    navigate(getCourseRoute(props.course.id, props.course.name));
  };

  return (
    <form onSubmit={form.onSubmit(onCreateLectureClick)} autoComplete='off'>
      <Fieldset>
        <Stack>
          <TextInput
            label={t('Lecture.Field.Name')}
            withAsterisk
            spellCheck='false'
            autoCorrect='off'
            {...form.getInputProps('name')}
          />
          <ControlledRichTextEditor
            label={t('Lecture.Field.Content')}
            inputProps={form.getInputProps('content')}
            setValue={(value) => form.setFieldValue('content', value)}
            isRequired
          />
          <Button type='submit' color='teal' variant={buttonVariant}>
            {t('Lecture.Action.CreateLecture')}
          </Button>
        </Stack>
      </Fieldset>
    </form>
  );
};
