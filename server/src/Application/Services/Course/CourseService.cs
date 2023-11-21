using StudyZen.Application.Dtos;
using StudyZen.Application.Exceptions;
using StudyZen.Application.Repositories;
using StudyZen.Application.Validation;
using StudyZen.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public sealed class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ValidationHandler _validationHandler;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public CourseService(IUnitOfWork unitOfWork, ValidationHandler validationHandler, ICurrentUserAccessor currentUserAccessor)
    {
        _unitOfWork = unitOfWork;
        _validationHandler = validationHandler;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<CourseDto> CreateCourse(CreateCourseDto dto)
    {
        await _validationHandler.ValidateAsync(dto);

        var newCourse = new Course(dto.Name, dto.Description);

        _unitOfWork.Courses.Add(newCourse);
        await _unitOfWork.SaveChanges();

        return new CourseDto(newCourse);
    }

    public async Task<CourseDto> GetCourseById(int id)
    {
        var course = await _unitOfWork.Courses.GetByIdChecked(id);
        return new CourseDto(course);
    }

    public async Task<IReadOnlyCollection<CourseDto>> GetAllCourses()
    {
        var allCourses = await _unitOfWork.Courses.Get();
        return allCourses.Select(course => new CourseDto(course)).ToList();
    }

    public async Task UpdateCourse(int id, UpdateCourseDto dto)
    {
        await _validationHandler.ValidateAsync(dto);

        var course = await _unitOfWork.Courses.GetByIdChecked(id);

        var applicationUserId = _currentUserAccessor.GetUserId();
        if (!course.CreatedBy.User.Equals(applicationUserId))
        {
            throw new AccessDeniedException();
        }

        course.Name = dto.Name ?? course.Name;
        course.Description = dto.Description ?? course.Description;

        await _unitOfWork.SaveChanges();
    }

    public async Task DeleteCourse(int id)
    {
        var course = await _unitOfWork.Courses.GetByIdChecked(id);

        var applicationUserId = _currentUserAccessor.GetUserId();
        if (!course.CreatedBy.User.Equals(applicationUserId))
        {
            throw new AccessDeniedException();
        }

        _unitOfWork.Courses.Delete(course);
        await _unitOfWork.SaveChanges();
    }
}