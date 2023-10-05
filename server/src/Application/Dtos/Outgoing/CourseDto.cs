using StudyZen.Domain.Entities;

namespace StudyZen.Application.Dtos;

public record CourseDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }

    public CourseDto(Course course)
    {
        Id = course.Id;
        Name = course.Name;
        Description = course.Description;
    }
}