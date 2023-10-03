using FluentValidation;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Validation.Validators;

public sealed class UpdateLectureRequestValidator : BaseValidator<UpdateLectureDto>
{
    public UpdateLectureRequestValidator()
    {
        RuleFor(lecture => lecture.Name)
        .Must(NullOrNotWhiteSpace)
        .WithMessage("Name must not be empty or whitespace!")
        .MaximumLength(50)
        .WithMessage("Name must not exceed 50 symbols!");
        RuleFor(lecture => lecture.Content)
        .Must(NullEmptyOrNotWhitespace)
        .WithMessage("Name must not be whitespace!")
        .MaximumLength(20000).WithMessage("Content must not exceed 20000 symbols!");
    }
}