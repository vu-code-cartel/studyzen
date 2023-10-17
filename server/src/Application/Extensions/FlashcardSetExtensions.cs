using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Extensions;

public static class FlashcardSetExtensions
{
    public static FlashcardSetDto ToDto(this FlashcardSet flashcardSet)
    {
        return new FlashcardSetDto(
            flashcardSet.Id,
            flashcardSet.LectureId,
            flashcardSet.Name,
            flashcardSet.Color);
    }

    public static List<FlashcardSetDto> ToDtos(this IEnumerable<FlashcardSet> flashcardSets)
    {
        return flashcardSets.Select(ToDto).ToList();
    }
}