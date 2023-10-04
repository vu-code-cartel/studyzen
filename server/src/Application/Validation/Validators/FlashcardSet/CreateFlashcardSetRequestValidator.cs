using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;

namespace StudyZen.Application.Validators;

public class CreateFlashcardSetRequestValidator : AbstractValidator<CreateFlashcardSetDto>
{
    private readonly ILectureService _lectureService;
    public CreateFlashcardSetRequestValidator(ILectureService lectureService)
    {
        _lectureService = lectureService;
        RuleFor(flashcardSet => flashcardSet.Name)
        .NotEmpty()
        .WithMessage("Name must not be empty!")
        .MaximumLength(50)
        .WithMessage("Name must not exceed 50 symbols!");
        RuleFor(flashcardSet => flashcardSet.LectureId)
        .Must(IsValidLectureId)
        .WithMessage("Lecture with the given id does not exist!");
    }
    protected bool IsValidLectureId(int? lectureId)
    {
        return lectureId is null || _lectureService.GetLectureById(lectureId ?? 0) is not null;
    }
}