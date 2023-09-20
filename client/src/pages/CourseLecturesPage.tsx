import { Button, Group, Paper, Stack, Text } from '@mantine/core';
import { LectureDto } from '../api/dtos';
import { CoursePageWrapper } from '../components/CoursePageWrapper';
import { useTranslation } from 'react-i18next';
import { Link, useParams } from 'react-router-dom';
import { getNewLectureRoute } from '../common/app-routes';
import { useSlugId } from '../hooks/useIdWithSlug';
import { NotFound } from '../components/NotFound';
import { useButtonVariant } from '../hooks/useButtonVariant';

const lectures: LectureDto[] = [
  {
    id: 1,
    name: 'Introduction to C#',
  },
  {
    id: 2,
    name: 'How to use Git',
  },
];

export const CourseLecturesPage = () => {
  const { t } = useTranslation();
  const { courseIdWithSlug } = useParams();
  const courseId = useSlugId(courseIdWithSlug);
  const buttonVariant = useButtonVariant();

  if (!courseIdWithSlug || courseId === null) {
    return <NotFound />;
  }

  return (
    <CoursePageWrapper tab='lectures' courseIdWithSlug={courseIdWithSlug} courseName='Software Engineering I'>
      <Group justify='end' mb='sm'>
        <Button color='teal' variant={buttonVariant} component={Link} to={getNewLectureRoute(courseIdWithSlug)}>
          {t('CoursePage.NewLecture')}
        </Button>
      </Group>
      <Stack gap='xs'>
        {lectures.map((lecture) => (
          <Paper withBorder p='md' shadow='xs'>
            <Text fw={600}>{lecture.name}</Text>
          </Paper>
        ))}
      </Stack>
    </CoursePageWrapper>
  );
};
