using StudyZen.Domain.Entities;

namespace StudyZen.Application.Dtos;

public class LectureDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }

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