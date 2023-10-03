using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Validation.Validators;

namespace StudyZen.Application.Validators;

public class UpdateCourseRequestValidator : BaseValidator<UpdateCourseDto>
{
    public UpdateCourseRequestValidator()
    {
        RuleFor(course => course.Name)
        .Must(NullOrNotWhiteSpace)
        .WithMessage("Name must not be empty or whitespace!")
        .MaximumLength(50)
        .WithMessage("Name must not exceed 50 symbols!");
        RuleFor(course => course.Description)
        .Must(NullEmptyOrNotWhitespace)
        .WithMessage("Description must not be whitespace!")
        .MaximumLength(200).
        WithMessage("Description must not exceed 200 symbols!");
    }
}