import { useParams } from 'react-router-dom';
import { CoursePageWrapper } from '../components/CoursePageWrapper';
import { usePageCategory } from '../hooks/usePageCategory';

export const CourseAboutPage = () => {
  const { courseIdWithSlug } = useParams();

  usePageCategory('courses');

  if (!courseIdWithSlug) {
    return <div>TODO: not found</div>;
  }

  return (
    <CoursePageWrapper tab='about' courseIdWithSlug={courseIdWithSlug} courseName='Software Engineering I'>
      about course
    </CoursePageWrapper>
  );
};
