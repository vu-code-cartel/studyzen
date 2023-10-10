using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;

namespace StudyZen.Application.Validation;

public class CreateFlashcardSetRequestValidator : AbstractValidator<CreateFlashcardSetDto>
{
    public CreateFlashcardSetRequestValidator(ILectureService lectureService)
    {
        RuleFor(fs => fs.Name)
            .FlashcardSetName();
        RuleFor(fs => fs.LectureId)
            .OptionalLectureId(lectureService);
    }
}