using StudyZen.Domain.Entities;

namespace StudyZen.Application.Dtos;

public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public static CourseDto ToDto(Course course)
    {
        return new CourseDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description
        };
    }
}