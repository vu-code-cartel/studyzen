import { useForm } from '@mantine/form';
import { CreateFlashcardSetRequest } from '../api/requests';
import { Color } from '../api/dtos';
import { Button, ComboboxItem, Fieldset, Select, Stack, Switch, TextInput } from '@mantine/core';
import { useTranslation } from 'react-i18next';
import { useEffect, useState } from 'react';
import { useGetCourses } from '../hooks/api/useCoursesApi';
import { CenteredLoader } from './CenteredLoader';
import { useGetLectures } from '../hooks/api/useLecturesApi';
import { useButtonVariant } from '../hooks/useButtonVariant';

interface FlashcardSetFormProps {
  onSubmit: (request: CreateFlashcardSetRequest) => void;
  isReadonly?: boolean;
  initialValues?: CreateFlashcardSetRequest;
}

export const FlashcardSetForm = (props: FlashcardSetFormProps) => {
  const { t } = useTranslation();
  const [color, setColor] = useState<string>(Color.Default.toString());
  const [courseId, setCourseId] = useState<string | null>(null);
  const [lectureId, setLectureId] = useState<string | null>(null);
  const [isConnected, setIsConnected] = useState<boolean>(false);
  const buttonVariant = useButtonVariant();
  const form = useForm<CreateFlashcardSetRequest>({
    initialValues: props.initialValues ?? {
      name: '',
      color: Color.Default,
    },
  });
  const { data: courses, isLoading: areCoursesLoading } = useGetCourses();
  const { data: lectures, isLoading: areLecturesLoading } = useGetLectures(courseId ? Number(courseId) : null);

  useEffect(() => {
    setLectureId(null);
  }, [courseId]);

  const onSubmit = (request: CreateFlashcardSetRequest) => {
    request.color = Color[color as keyof typeof Color];

    if (isConnected) {
      request.lectureId = isNaN(Number(lectureId)) ? undefined : Number(lectureId);
    }

    props.onSubmit(request);
  };

  if (areCoursesLoading) {
    return <CenteredLoader />;
  }

  return (
    <form onSubmit={form.onSubmit(onSubmit)} autoComplete='off'>
      <Fieldset>
        <Stack>
          <TextInput
            label={t('FlashcardSet.Field.Name')}
            placeholder={t('FlashcardSet.Placeholder.Name')}
            withAsterisk={!props.isReadonly}
            readOnly={props.isReadonly}
            {...form.getInputProps('name')}
          />
          <Select
            label={t('FlashcardSet.Field.Color')}
            data={[
              { value: Color.Default.toString(), label: t('App.Color.Default') },
              { value: Color.Blue.toString(), label: t('App.Color.Blue') },
              { value: Color.Green.toString(), label: t('App.Color.Green') },
              { value: Color.Red.toString(), label: t('App.Color.Red') },
              { value: Color.Purple.toString(), label: t('App.Color.Purple') },
              { value: Color.Yellow.toString(), label: t('App.Color.Yellow') },
            ]}
            value={color}
            onChange={(value) => (value ? setColor(value) : undefined)}
            allowDeselect={false}
            withAsterisk={!props.isReadonly}
            readOnly={props.isReadonly}
          />

          {courses && isConnected && (
            <>
              <Select
                label={t('FlashcardSet.Field.Course')}
                placeholder={t('FlashcardSet.Placeholder.ConnectFlashcardWithCourse')}
                data={courses.map((course): ComboboxItem => ({ label: course.name, value: course.id.toString() }))}
                value={courseId}
                onChange={setCourseId}
                readOnly={props.isReadonly}
                searchable
                nothingFoundMessage={t('App.NothingFound')}
              />

              {courseId && (
                <Select
                  label={t('FlashcardSet.Field.Lecture')}
                  placeholder={t('FlashcardSet.Placeholder.ConnectFlashcardWithLecture')}
                  data={
                    lectures
                      ? lectures.map((lecture): ComboboxItem => ({ label: lecture.name, value: lecture.id.toString() }))
                      : []
                  }
                  value={lectureId}
                  onChange={setLectureId}
                  readOnly={props.isReadonly}
                  searchable
                  nothingFoundMessage={
                    areLecturesLoading ? t('FlashcardSet.Placeholder.LoadingLectures') : t('App.NothingFound')
                  }
                />
              )}
            </>
          )}

          <Switch
            label={t('FlashcardSet.ConnectToCourseOrLecture')}
            labelPosition='left'
            ml='auto'
            checked={isConnected}
            onChange={(e) => setIsConnected(e.currentTarget.checked)}
          />

          <Button type='submit' variant={buttonVariant} color='teal' disabled={props.isReadonly}>
            {t('FlashcardSet.Action.CreateFlashcardSet')}
          </Button>
        </Stack>
      </Fieldset>
    </form>
  );
};
