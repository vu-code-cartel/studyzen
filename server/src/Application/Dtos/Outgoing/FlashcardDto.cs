using StudyZen.Domain.Entities;

namespace StudyZen.Application.Dtos;

public record FlashcardDto 
{
    public int Id { get; init; }
    public int FlashcardSetId { get; init; }
    public string Question { get; init; }
    public string Answer { get; init; }

    public FlashcardDto(Flashcard flashcard)
    {
        Id = flashcard.Id;
        FlashcardSetId = flashcard.FlashcardSetId;
        Question = flashcard.Question;
        Answer = flashcard.Answer;
    }
}