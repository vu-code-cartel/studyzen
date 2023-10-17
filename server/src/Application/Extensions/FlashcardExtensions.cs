using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Extensions;

public static class FlashcardExtensions
{
    public static FlashcardDto ToDto(this Flashcard flashcard)
    {
        return new FlashcardDto(
            flashcard.Id,
            flashcard.FlashcardSetId,
            flashcard.Front,
            flashcard.Back);
    }

    public static List<FlashcardDto> ToDtos(this IEnumerable<Flashcard> flashcards)
    {
        return flashcards.Select(ToDto).ToList();
    }
}