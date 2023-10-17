using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Extensions;

public static class CourseExtensions
{
    public static CourseDto ToDto(this Course course)
    {
        return new CourseDto(
            course.Id,
            course.Name,
            course.Description);
    }

    public static List<CourseDto> ToDtos(this IEnumerable<Course> courses)
    {
        return courses.Select(ToDto).ToList();
    }
}