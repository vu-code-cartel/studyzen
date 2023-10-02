using FluentValidation;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Validators;

public class UpdatedLectureValidator : AbstractValidator<Lecture>
{
    public UpdatedLectureValidator()
    {
        RuleFor(lecture => lecture.Name).NotEmpty().WithMessage("Name must not be empty!");
    }
}