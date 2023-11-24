import { Button, Grid, Group, Modal, Stack, Tabs, Text, TextInput } from '@mantine/core';
import { useTranslation } from 'react-i18next';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { AppRoutes, getCourseRoute, getLectureRoute, getNewLectureRoute } from '../../common/app-routes';
import { NotFound } from '../../components/NotFound';
import { useButtonVariant } from '../../hooks/useButtonVariant';
import { useGetLectures } from '../../hooks/api/useLecturesApi';
import { CenteredLoader } from '../../components/CenteredLoader';
import { useDeleteCourse, useGetCourse, useUpdateCourse } from '../../hooks/api/useCoursesApi';
import { getIdFromSlug } from '../../common/utils';
import { useDisclosure, useDocumentTitle } from '@mantine/hooks';
import { usePageCategory } from '../../hooks/usePageCategory';
import { useEffect, useState } from 'react';
import { CourseDto, LectureDto } from '../../api/dtos';
import { PageContainer } from '../../components/PageContainer';
import { PageHeader } from '../../components/PageHeader';
import { AppBreadcrumbs } from '../../components/AppBreadcrumbs';
import { t } from 'i18next';
import { StyledList } from '../../components/StyledList';
import { formatDistanceToNow } from 'date-fns';
import { CourseForm } from '../../components/CourseForm';
import { CreateCourseRequest, UpdateCourseRequest } from '../../api/requests';

class CourseTabs {
  public static readonly Lectures = 'lectures';
  public static readonly About = 'about';
  public static readonly Settings = 'settings';
}

export const CoursePage = () => {
  const { t } = useTranslation();
  const { courseIdWithSlug } = useParams();
  const { data: course, isLoading: isCourseLoading } = useGetCourse(getIdFromSlug(courseIdWithSlug));

  useDocumentTitle(t('Course.DocumentTitle.Course', { courseName: course?.name }));
  usePageCategory('courses');

  if (isCourseLoading) {
    return <CenteredLoader />;
  }

  if (!course) {
    return <NotFound />;
  }

  return (
    <PageContainer>
      <PageHeader>
        <AppBreadcrumbs
          items={[
            { title: t('Course.Title.Courses'), to: AppRoutes.Courses },
            { title: course.name, to: getCourseRoute(course.id, course.name) },
          ]}
        />
      </PageHeader>

      <Tabs defaultValue={CourseTabs.Lectures} mb='sm'>
        <Tabs.List mb='md'>
          <Tabs.Tab value={CourseTabs.Lectures}>{t('Course.Tab.Lectures')}</Tabs.Tab>
          <Tabs.Tab value={CourseTabs.About}>{t('Course.Tab.About')}</Tabs.Tab>
          <Tabs.Tab value={CourseTabs.Settings}>{t('Course.Tab.Settings')}</Tabs.Tab>
        </Tabs.List>

        <LecturesPanel course={course} />
        <AboutCoursePanel course={course} />
        <CourseSettingsPanel course={course} />
      </Tabs>
    </PageContainer>
  );
};

interface CoursePanelProps {
  course: CourseDto;
}

const LecturesPanel = (props: CoursePanelProps) => {
  const [lectureNameFilter, setLectureNameFilter] = useState<string>('');
  const [filteredLectures, setFilteredLectures] = useState<LectureDto[]>([]);
  const buttonVariant = useButtonVariant();
  const { data: lectures, isLoading: areLecturesLoading } = useGetLectures(props.course.id);

  useEffect(() => {
    if (!lectures) {
      return;
    }

    setFilteredLectures(
      lectures.filter((lecture) => lecture.name.toLowerCase().includes(lectureNameFilter.toLowerCase())),
    );
  }, [lectures, lectureNameFilter]);

  if (areLecturesLoading) {
    return <CenteredLoader />;
  }

  return (
    <Tabs.Panel value={CourseTabs.Lectures}>
      <Grid mb='md' justify='space-between'>
        <Grid.Col span='auto'>
          <TextInput
            placeholder='Search lectures...'
            value={lectureNameFilter}
            onChange={(e) => setLectureNameFilter(e.currentTarget.value)}
          />
        </Grid.Col>
        <Grid.Col span='content'>
          <Button
            fullWidth
            variant={buttonVariant}
            component={Link}
            to={getNewLectureRoute(props.course.id, props.course.name)}
          >
            {t('Lecture.Action.NewLecture')}
          </Button>
        </Grid.Col>
      </Grid>
      <StyledList
        items={filteredLectures.map((lecture) => (
          <>
            <Text
              fw={600}
              component={Link}
              to={getLectureRoute(props.course.id, props.course.name, lecture.id, lecture.name)}
            >
              {lecture.name}
            </Text>
            <Text size='xs' opacity={0.6}>
              {t('App.Stamp.UpdatedBy')} {lecture.updatedBy.user}{' '}
              {formatDistanceToNow(lecture.updatedBy.timestamp, { addSuffix: true })}
            </Text>
          </>
        ))}
      ></StyledList>
    </Tabs.Panel>
  );
};

const AboutCoursePanel = (props: CoursePanelProps) => {
  const [isEditing, setIsEditing] = useState<boolean>(false);
  const courseUpdateMutation = useUpdateCourse();

  const onUpdateCourseClick = async (request: CreateCourseRequest) => {
    const updateRequest: UpdateCourseRequest = {
      name: props.course.name == request.name ? null : request.name,
      description: props.course.description == request.description ? null : request.description,
    };

    await courseUpdateMutation.mutateAsync({ courseId: props.course.id, request: updateRequest });
    setIsEditing(false);
  };

  return (
    <Tabs.Panel value={CourseTabs.About}>
      <CourseForm
        onSubmit={onUpdateCourseClick}
        onEditClick={() => setIsEditing(true)}
        onCancelClick={() => setIsEditing(false)}
        isReadonly={!isEditing}
        isEditable
        initialValues={{ name: props.course.name, description: props.course.description }}
      />
    </Tabs.Panel>
  );
};

const CourseSettingsPanel = (props: CoursePanelProps) => {
  const { t } = useTranslation();
  const [isDeleteModalOpen, { open: openDeleteModal, close: closeDeleteModal }] = useDisclosure(false);
  const deleteCourseMutation = useDeleteCourse();
  const navigate = useNavigate();

  const onDeleteCourseClick = async () => {
    await deleteCourseMutation.mutateAsync(props.course.id);
    navigate(AppRoutes.Courses);
  };

  return (
    <Tabs.Panel value={CourseTabs.Settings}>
      <StyledList
        items={[
          <Group justify='space-between'>
            <Text>{t('Course.Settings.DeleteCourse')}</Text>
            <Button variant='light' color='red' onClick={openDeleteModal}>
              {t('Course.Settings.Delete')}
            </Button>
          </Group>,
        ]}
      ></StyledList>

      <Modal opened={isDeleteModalOpen} onClose={closeDeleteModal} title={t('Course.Settings.DeleteCourse')}>
        <Stack>
          <Text>{t('Course.Settings.DeleteConfirmPrompt', { courseName: props.course.name })}</Text>
          <Group justify='end'>
            <Button variant='default' onClick={() => onDeleteCourseClick()}>
              {t('App.Action.Yes')}
            </Button>
            <Button variant='default' onClick={closeDeleteModal}>
              {t('App.Action.No')}
            </Button>
          </Group>
        </Stack>
      </Modal>
    </Tabs.Panel>
  );
};
