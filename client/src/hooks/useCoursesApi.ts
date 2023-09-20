import { SERVER_URL, axiosClient } from '../api/config';
import { CourseDto } from '../api/dtos';
import { CreateCourseRequest, UpdateCourseRequest } from '../api/requests';

export const useCoursesApi = () => {
  const url = `${SERVER_URL}/courses`;

  const createCourse = async (request: CreateCourseRequest): Promise<void> => {
    await axiosClient.post(url, request);
  };

  const getCourses = async (): Promise<CourseDto[]> => {
    const response = await axiosClient.get<CourseDto[]>(url);
    return response.data;
  };

  const getCourse = async (courseId: number): Promise<CourseDto> => {
    const response = await axiosClient.get(`${url}/${courseId}`);
    return response.data;
  };

  const updateCourse = async (courseId: number, request: UpdateCourseRequest): Promise<void> => {
    await axiosClient.patch(`${url}/${courseId}`, request);
  };

  const deleteCourse = async (courseId: number): Promise<void> => {
    await axiosClient.delete(`${url}/${courseId}`);
  };

  return { createCourse, getCourses, getCourse, updateCourse, deleteCourse };
};
