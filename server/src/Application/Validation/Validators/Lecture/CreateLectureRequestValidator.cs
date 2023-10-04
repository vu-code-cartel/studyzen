using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;

namespace StudyZen.Application.Validation.Validators;

public class CreateLectureRequestValidator : AbstractValidator<CreateLectureDto>
{
    private readonly ICourseService _courseService;
    public CreateLectureRequestValidator(ICourseService courseService)
    {
        _courseService = courseService;
        RuleFor(lecture => lecture.Name)
        .NotEmpty()
        .WithMessage("Name must not be empty!")
        .MaximumLength(50)
        .WithMessage("Name must not exceed 50 symbols!");
        RuleFor(lecture => lecture.Content)
        .NotNull()
        .WithMessage("Content cannot be null!")
        .MaximumLength(20000)
        .WithMessage("Content must not exceed 20000 symbols!");
        RuleFor(lecture => lecture.CourseId)
        .Must(IsValidCourseId)
        .WithMessage("Course with the given id does not exist!");
    }
    protected bool IsValidCourseId(int courseId)
    {
        return _courseService.GetCourseById(courseId) is not null;
    }
}