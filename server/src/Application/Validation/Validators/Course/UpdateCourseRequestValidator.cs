using FluentValidation;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Validation;

public class UpdateCourseRequestValidator : AbstractValidator<UpdateCourseDto>
{
    public UpdateCourseRequestValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .Unless(c => c.Name is null)
            .WithMessage("Name must not be empty or whitespace!")
            .MaximumLength(50)
            .WithMessage("Name must not exceed 50 symbols!");
        RuleFor(course => course.Description)
            .NotEmpty()
            .Unless(c => c.Description is null || c.Description.Equals(""))
            .WithMessage("Description must not be whitespace!")
            .MaximumLength(200).WithMessage("Description must not exceed 200 symbols!");
    }
}