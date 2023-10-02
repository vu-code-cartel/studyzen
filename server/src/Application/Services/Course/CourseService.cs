﻿using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public sealed class CourseService : ICourseService
{
    private readonly ICourseRepository _courses;
    private readonly ILectureRepository _lectures;

    public CourseService(ICourseRepository courses, ILectureRepository lectures)
    {
        _courses = courses;
        _lectures = lectures;
    }

    public Course CreateCourse(CreateCourseDto dto)
    {
        var newCourse = new Course(dto.Name, dto.Description);
        _courses.Add(newCourse);
        return newCourse;
    }

    public Course? GetCourseById(int id)
    {
        var course = _courses.GetById(id);
        return course;
    }

    public bool UpdateCourse(int id, UpdateCourseDto dto)
    {
        var course = _courses.GetById(id);
        if (course is null)
        {
            return false;
        }

        course.Name = dto.Name ?? course.Name;
        course.Description = dto.Description ?? course.Description;
        _courses.Update(course);

        return true;
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