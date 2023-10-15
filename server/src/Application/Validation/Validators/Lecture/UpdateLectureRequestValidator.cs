using FluentValidation;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Validation;

public sealed class UpdateLectureRequestValidator : AbstractValidator<UpdateLectureDto>
{
    public UpdateLectureRequestValidator()
    {
        RuleFor(l => l.Name)
            .LectureName()
            .Unless(l => l.Name is null);
        RuleFor(lecture => lecture.Content)
            .LectureContent()
            .Unless(l => l.Content is null);
    }
}