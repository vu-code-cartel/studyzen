import { SERVER_URL, axiosClient } from '../api/config';
import { LectureDto } from '../api/dtos';
import { CreateLectureRequest, UpdateLectureRequest } from '../api/requests';

export const useLecturesApi = () => {
  const url = `${SERVER_URL}/lectures`;

  const createLecture = async (request: CreateLectureRequest): Promise<void> => {
    await axiosClient.post(url, request);
  };

  const getLectures = async (courseId: number): Promise<LectureDto[]> => {
    const response = await axiosClient.get<LectureDto[]>(`${url}/${courseId}`);
    return response.data;
  };

  const getLecture = async (lectureId: number): Promise<LectureDto> => {
    const response = await axiosClient.get<LectureDto>(`${url}/${lectureId}`);
    return response.data;
  };

  const updateLecture = async (lectureId: number, request: UpdateLectureRequest): Promise<void> => {
    await axiosClient.patch(`${url}/${lectureId}`, request);
  };

  const deleteLecture = async (lectureId: number): Promise<void> => {
    await axiosClient.delete(`${url}/${lectureId}`);
  };

  return {
    createLecture,
    getLectures,
    getLecture,
    updateLecture,
    deleteLecture,
  };
};
