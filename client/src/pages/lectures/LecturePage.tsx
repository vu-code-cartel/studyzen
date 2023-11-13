import { Button, Group, Modal, Stack, Tabs, Text } from '@mantine/core';
import { PageHeader } from '../../components/PageHeader';
import { useNavigate, useParams } from 'react-router-dom';
import { AppBreadcrumbs } from '../../components/AppBreadcrumbs';
import { AppRoutes, getCourseRoute, getLectureRoute } from '../../common/app-routes';
import { useGetCourse } from '../../hooks/api/useCoursesApi';
import { getIdFromSlug } from '../../common/utils';
import { useDeleteLecture, useGetLecture, useUpdateLecture } from '../../hooks/api/useLecturesApi';
import { CenteredLoader } from '../../components/CenteredLoader';
import { NotFound } from '../../components/NotFound';
import { useTranslation } from 'react-i18next';
import { useDisclosure, useDocumentTitle } from '@mantine/hooks';
import { usePageCategory } from '../../hooks/usePageCategory';
import { PageContainer } from '../../components/PageContainer';
import { ControlledRichTextEditor } from '../../components/ControlledRichTextEditor';
import { useState } from 'react';
import { CourseDto, LectureDto } from '../../api/dtos';
import { UpdateLectureRequest } from '../../api/requests';
import { Editor } from '@tiptap/react';
import { useAppStore } from '../../hooks/useAppStore';
import { StyledList } from '../../components/StyledList';
import { useGetFlashcardSets } from '../../hooks/api/useFlashcardSetsApi';
import { FlashcardSetList } from '../../components/FlashcardSetList';

class LectureTabs {
  public static readonly Lecture = 'lecture';
  public static readonly Settings = 'settings';
  public static readonly Flashcards = 'flashcards';
}

export const LecturePage = () => {
  const { courseIdWithSlug, lectureIdWithSlug } = useParams();
  const { data: course, isLoading: isCourseLoading } = useGetCourse(getIdFromSlug(courseIdWithSlug));
  const { data: lecture, isLoading: isLectureLoading } = useGetLecture(getIdFromSlug(lectureIdWithSlug));
  const { t } = useTranslation();
  const isMobile = useAppStore((state) => state.isMobile);
  useDocumentTitle(t('Lecture.DocumentTitle.Lecture', { courseName: course?.name, lectureName: lecture?.name }));
  usePageCategory('courses');

  if (isCourseLoading || isLectureLoading) {
    return <CenteredLoader />;
  }

  if (!course || !lecture) {
    return <NotFound />;
  }

  return (
    <PageContainer>
      <PageHeader>
        <AppBreadcrumbs
          items={[
            { title: t('Course.Title.Courses'), to: AppRoutes.Courses },
            { title: course.name, to: getCourseRoute(course.id, course.name) },
            {
              title: lecture.name,
              to: getLectureRoute(course.id, course.name, lecture.id, lecture.name),
            },
          ]}
        />
      </PageHeader>

      <Tabs
        orientation={isMobile ? 'horizontal' : 'vertical'}
        defaultValue={LectureTabs.Lecture}
        placement='right'
        color='teal'
        keepMounted={false}
      >
        <Tabs.List ml={isMobile ? 0 : 'md'} mb={isMobile ? 'md' : 0}>
          <Tabs.Tab value={LectureTabs.Lecture}>{t('Lecture.Tab.Lecture')}</Tabs.Tab>
          <Tabs.Tab value={LectureTabs.Flashcards}>{t('Lecture.Tab.Flashcards')}</Tabs.Tab>
          <Tabs.Tab value={LectureTabs.Settings}>{t('Lecture.Tab.Settings')}</Tabs.Tab>
        </Tabs.List>

        <Tabs.Panel value={LectureTabs.Lecture}>
          <LecturePanel course={course} lecture={lecture} />
        </Tabs.Panel>
        <Tabs.Panel value={LectureTabs.Flashcards}>
          <LectureFlashcardsPanel course={course} lecture={lecture} />
        </Tabs.Panel>
        <Tabs.Panel value={LectureTabs.Settings}>
          <LectureSettingsPanel course={course} lecture={lecture} />
        </Tabs.Panel>
      </Tabs>
    </PageContainer>
  );
};

interface LecturePanelProps {
  course: CourseDto;
  lecture: LectureDto;
}

const LecturePanel = (props: LecturePanelProps) => {
  const [content, setContent] = useState<string>(props.lecture.content);
  const [isEditorReadonly, setIsEditorReadonly] = useState<boolean>(true);
  const updateLectureMutation = useUpdateLecture();

  const onSaveEditorChangesClick = async () => {
    const request: UpdateLectureRequest = {
      name: null,
      content: content,
    };

    await updateLectureMutation.mutateAsync({ courseId: props.course.id, lectureId: props.lecture.id, request });
  };

  const onEditContentClick = (editor: Editor | null) => {
    if (!isEditorReadonly) {
      // Revert the unsaved content
      editor?.commands.setContent(props.lecture.content);
    }

    setIsEditorReadonly((prev) => !prev);
  };

  return (
    <>
      <ControlledRichTextEditor
        content={content}
        setValue={setContent}
        isReadonly={isEditorReadonly}
        showEditContentControl
        onEditContentClick={onEditContentClick}
        onSaveChangesClick={onSaveEditorChangesClick}
      />
    </>
  );
};

const LectureSettingsPanel = (props: LecturePanelProps) => {
  const { t } = useTranslation();
  const [isDeleteModalOpen, { open: openDeleteModal, close: closeDeleteModal }] = useDisclosure(false);
  const deleteLectureMutation = useDeleteLecture();
  const navigate = useNavigate();

  const onDeleteLectureClick = async () => {
    await deleteLectureMutation.mutateAsync({ courseId: props.course.id, lectureId: props.lecture.id });
    navigate(getCourseRoute(props.course.id, props.course.name));
  };

  return (
    <>
      <StyledList
        items={[
          <Group justify='space-between'>
            <Text>{t('Lecture.Settings.DeleteLecture')}</Text>
            <Button variant='light' color='red' onClick={openDeleteModal}>
              {t('Lecture.Settings.Delete')}
            </Button>
          </Group>,
        ]}
      ></StyledList>

      <Modal opened={isDeleteModalOpen} onClose={closeDeleteModal} title={t('Lecture.Settings.DeleteLecture')}>
        <Stack>
          <Text>{t('Lecture.Settings.DeleteConfirmPrompt', { lectureName: props.lecture.name })}</Text>
          <Group justify='end'>
            <Button variant='default' onClick={() => onDeleteLectureClick()}>
              {t('App.Action.Yes')}
            </Button>
            <Button variant='default' onClick={closeDeleteModal}>
              {t('App.Action.No')}
            </Button>
          </Group>
        </Stack>
      </Modal>
    </>
  );
};

const LectureFlashcardsPanel = (props: LecturePanelProps) => {
  const { isLoading: areFlashcardSetsLoading, data: flashcardSets } = useGetFlashcardSets(props.lecture.id);

  if (areFlashcardSetsLoading) {
    return <CenteredLoader />;
  }

  return <Stack>{flashcardSets && <FlashcardSetList flashcardSets={flashcardSets} />}</Stack>;
};
