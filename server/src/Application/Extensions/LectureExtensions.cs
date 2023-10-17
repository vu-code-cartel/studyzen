using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Extensions;

public static class LectureExtensions
{
    public static LectureDto ToDto(this Lecture lecture)
    {
        return new LectureDto(
            lecture.Id,
            lecture.CourseId,
            lecture.Name,
            lecture.Content,
            lecture.CreatedBy,
            lecture.UpdatedBy);
    }

    public static List<LectureDto> ToDtos(this IEnumerable<Lecture> lectures)
    {
        return lectures.Select(ToDto).ToList();
    }
}