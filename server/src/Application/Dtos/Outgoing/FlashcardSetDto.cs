using StudyZen.Domain.Enums;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Dtos;

public record FlashcardSetDto 
{
    public int Id { get; init; }
    public int? LectureId { get; init; }
    public string Name { get; init; }
    public Color Color { get; init; }

     public FlashcardSetDto(FlashcardSet flashcardSet)
    {
        Id = flashcardSet.Id;
        LectureId = flashcardSet.LectureId;
        Name = flashcardSet.Name;
        Color = flashcardSet.Color;
    }

}