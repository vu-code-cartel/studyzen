using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface ICourseService
{
    CourseDto CreateCourse(CreateCourseDto dto);
    CourseDto? GetCourseById(int id);
    IReadOnlyCollection<CourseDto> GetAllCourses();
    bool UpdateCourse(int id, UpdateCourseDto dto);
    bool DeleteCourse(int id);
}