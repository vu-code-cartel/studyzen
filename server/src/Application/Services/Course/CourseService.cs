using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using FluentValidation;
using StudyZen.Application.Validation;

namespace StudyZen.Application.Services;

public sealed class CourseService : ICourseService
{
    private readonly ICourseRepository _courses;
    private readonly ILectureRepository _lectures;
    private readonly ValidationHandler _validationHandler;

    public CourseService(ICourseRepository courses, ILectureRepository lectures, ValidationHandler validationHandler)
    {
        _courses = courses;
        _lectures = lectures;
        _validationHandler = validationHandler;
    }

    public Course CreateCourse(CreateCourseDto dto)
    {
        _validationHandler.Validate(dto);
        var newCourse = new Course(dto.Name, dto.Description);
        _courses.Add(newCourse);
        return newCourse;
    }

    public Course? GetCourseById(int id)
    {
        var course = _courses.GetById(id);
        return course;
    }

    public Course? UpdateCourse(int id, UpdateCourseDto dto)
    {
        var course = _courses.GetById(id);
        if (course is null)
        {
            return null;
        }

        _validationHandler.Validate(dto);
        course.Name = dto.Name ?? course.Name;
        course.Description = dto.Description ?? course.Description;
        _courses.Update(course);

        return course;
    }

    public void DeleteCourse(int id)
    {
        DeleteLecturesByCourseId(id);
        _courses.Delete(id);
    }

    public IReadOnlyCollection<Course> GetAllCourses()
    {
        var allCourses = _courses.GetAll();
        return allCourses;
    }

    private void DeleteLecturesByCourseId(int courseId)
    {
        var allLectures = _lectures.GetAll();
        var courseLectures = allLectures.Where(l => l.CourseId == courseId);

        foreach (var courseLecture in courseLectures)
        {
            _lectures.Delete(courseLecture.Id);
        }
    }
}