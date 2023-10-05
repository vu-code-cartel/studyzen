using StudyZen.Domain.Entities;

namespace StudyZen.Application.Dtos;

public record LectureDto
{
    public int Id { get; init; }
    public int CourseId { get; init; }
    public string Name { get; init; }
    public string Content { get; init; }

    public LectureDto(Lecture lecture)
    {
        Id = lecture.Id;
        CourseId = lecture.CourseId;
        Name = lecture.Name;
        Content = lecture.Content;
    }
    
}