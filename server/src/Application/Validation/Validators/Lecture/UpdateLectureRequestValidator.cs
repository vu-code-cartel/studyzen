using FluentValidation;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Validation.Validators;

public sealed class UpdateLectureRequestValidator : AbstractValidator<UpdateLectureDto>
{
    public UpdateLectureRequestValidator()
    {
        RuleFor(l => l.Name)
        .NotEmpty()
        .Unless(l => l.Name is null)
        .WithMessage("Name must not be empty or whitespace!")
        .MaximumLength(50)
        .WithMessage("Name must not exceed 50 symbols!");
        RuleFor(lecture => lecture.Content)
        .NotEmpty()
        .Unless(l => l.Content is null || l.Content.Equals(""))
        .WithMessage("Name must not be whitespace!")
        .MaximumLength(20000).WithMessage("Content must not exceed 20000 symbols!");
    }
}