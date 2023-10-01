using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public interface ICourseService
{
    CourseDto CreateCourse(CreateCourseDto dto);
    CourseDto? GetCourseById(int id);
    IReadOnlyCollection<CourseDto> GetAllCourses();
    CourseDto? UpdateCourse(int id, UpdateCourseDto dto);
    void DeleteCourse(int id);
}