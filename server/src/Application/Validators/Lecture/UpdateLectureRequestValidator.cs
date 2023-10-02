using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;

namespace StudyZen.Application.Validators;

public class UpdateLectureRequestValidator : AbstractValidator<UpdateLectureDto>
{
    public UpdateLectureRequestValidator()
    {
        RuleFor(lecture => lecture.Name).MaximumLength(50).WithMessage("Name must not exceed 50 symbols!");
        RuleFor(lecture => lecture.Content).MaximumLength(20000).WithMessage("Content must not exceed 20000 symbols!");
    }
}