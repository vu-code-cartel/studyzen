using StudyZen.Domain.Enums;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Dtos;

public class FlashcardSetDto 
{
    public int Id { get; set; }
    public int? LectureId { get; set; }
    public string Name { get; set; }
    public Color Color { get; set; }

    public static FlashcardSetDto ToDto(FlashcardSet flashcardSet)
    {
        return new FlashcardSetDto
        {
            Id = flashcardSet.Id,
            LectureId = flashcardSet.LectureId,
            Name = flashcardSet.Name,
            Color = flashcardSet.Color
        };
    }
}