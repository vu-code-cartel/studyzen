using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;

namespace StudyZen.Application.Validation;

public class CreateLectureRequestValidator : AbstractValidator<CreateLectureDto>
{
    public CreateLectureRequestValidator(ICourseService courseService)
    {
        RuleFor(l => l.Name)
            .LectureName();
        RuleFor(l => l.Content)
            .LectureContent();
    }
}