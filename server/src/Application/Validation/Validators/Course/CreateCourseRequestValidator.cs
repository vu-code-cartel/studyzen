using FluentValidation;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Validators;

public class CreateCourseRequestValidator : AbstractValidator<CreateCourseDto>
{
    public CreateCourseRequestValidator()
    {
        RuleFor(course => course.Name)
        .NotEmpty()
        .WithMessage("Name must not be empty!")
        .MaximumLength(50)
        .WithMessage("Name must not exceed 50 symbols!");
        RuleFor(course => course.Description)
        .NotNull()
        .WithMessage("Description cannot be null!")
        .MaximumLength(200)
        .WithMessage("Content must not exceed 200 symbols!");
    }
}