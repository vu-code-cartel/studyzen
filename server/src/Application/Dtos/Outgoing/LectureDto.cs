using StudyZen.Domain.Entities;
using StudyZen.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace StudyZen.Application.Dtos;

public record LectureDto
{
    public int Id { get; init; }
    public int CourseId { get; init; }
    public string Name { get; init; }
    public string Content { get; init; }
    public UserActionStamp CreatedBy { get; init; }
    public UserActionStamp UpdatedBy { get; init; }

    public LectureDto(Lecture lecture)
    {
        Id = lecture.Id;
        CourseId = lecture.CourseId;
        Name = lecture.Name;
        Content = lecture.Content;
        CreatedBy = lecture.CreatedBy;
        UpdatedBy = lecture.UpdatedBy;
    }
    [JsonConstructor]
    public LectureDto() { }
}