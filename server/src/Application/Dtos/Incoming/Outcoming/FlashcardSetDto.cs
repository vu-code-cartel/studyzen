using StudyZen.Domain.Enums;

namespace StudyZen.Application.Dtos;

public class FlashcardSetDto 
{
    public int? LectureId { get; set; }
    public string Name { get; set; }
    public Color Color { get; set; }
}