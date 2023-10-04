using StudyZen.Domain.Entities;

namespace StudyZen.Application.Dtos;

public class LectureDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }

    public LectureDto(Lecture lecture)
    {
        Id = lecture.Id;
        CourseId = lecture.CourseId;
        Name = lecture.Name;
        Content = lecture.Content;
    }

    public static LectureDto toDto(Lecture lecture)
    {
        return new LectureDto(lecture);
    }
    
}