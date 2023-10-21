using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface ICourseService
{
    Task<CourseDto> CreateCourse(CreateCourseDto dto);
    Task<CourseDto> GetCourseById(int id);
    Task<IReadOnlyCollection<CourseDto>> GetAllCourses();
    Task UpdateCourse(int id, UpdateCourseDto dto);
    Task DeleteCourse(int id);
}