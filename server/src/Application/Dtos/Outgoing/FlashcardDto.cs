using StudyZen.Domain.Entities;
using System.Text.Json.Serialization;

namespace StudyZen.Application.Dtos;

public record FlashcardDto
{
    public int Id { get; init; }
    public int FlashcardSetId { get; init; }
    public string Front { get; init; }
    public string Back { get; init; }

    public FlashcardDto(Flashcard flashcard)
    {
        Id = flashcard.Id;
        FlashcardSetId = flashcard.FlashcardSetId;
        Front = flashcard.Front;
        Back = flashcard.Back;
    }
    [JsonConstructor]
    public FlashcardDto() { }
}