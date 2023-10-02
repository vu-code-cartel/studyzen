using FluentValidation;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Validators;

public class UpdateCourseRequestValidator : AbstractValidator<UpdateCourseDto>
{
    public UpdateCourseRequestValidator()
    {
        RuleFor(course => course.Name).MaximumLength(50).WithMessage("Name must not exceed 50 symbols!");
        RuleFor(course => course.Description).MaximumLength(200).WithMessage("Description must not exceed 200 symbols!");
    }
}