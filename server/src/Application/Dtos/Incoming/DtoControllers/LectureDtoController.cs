using StudyZen.Domain.Entities;
using StudyZen.Application.Dtos;

namespace StudyZen.DtoControllers
{
    public class LectureDtoController
    {
        public static LectureDto ToDto(Lecture lecture)
        {
            return new LectureDto
            {
                Id = lecture.Id,
                CourseId = lecture.CourseId,
                Name = lecture.Name,
                Content = lecture.Content
            };
        }
    }
}
