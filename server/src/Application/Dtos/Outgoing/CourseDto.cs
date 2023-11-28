using System.Text.Json.Serialization;
using StudyZen.Domain.Entities;
using StudyZen.Domain.ValueObjects;

namespace StudyZen.Application.Dtos;

public record CourseDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public UserActionStamp CreatedBy { get; init; }
    public UserActionStamp UpdatedBy { get; init; }

    public CourseDto(Course course)
    {
        Id = course.Id;
        Name = course.Name;
        Description = course.Description;
        CreatedBy = course.CreatedBy;
        UpdatedBy = course.UpdatedBy;
    }
    [JsonConstructor]
    public CourseDto() { }
}