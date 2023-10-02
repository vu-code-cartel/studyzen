using FluentValidation;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Validators;

public class UpdatedCourseValidator : AbstractValidator<Course>
{
    public UpdatedCourseValidator()
    {
        RuleFor(course => course.Name).NotEmpty().WithMessage("Name must not be empty!");
    }
}