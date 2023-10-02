using StudyZen.Domain.Entities;

namespace StudyZen.Application.Dtos;

public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public CourseDto(Course course)
    {
        Id = course.Id;
        Name = course.Name;
        Description = course.Description;
    }

    public static CourseDto ToDto(Course course)
    {
        return new CourseDto(course);
    }
}