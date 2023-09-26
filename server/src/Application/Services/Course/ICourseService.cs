using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public interface ICourseService
{
    Course CreateCourse(CreateCourseDto dto);
    Course? GetCourseById(int id);
    IReadOnlyCollection<Course> GetAllCourses();
    Course? UpdateCourse(int id, UpdateCourseDto dto);
    void DeleteCourse(int id);
}