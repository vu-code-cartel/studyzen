using StudyZen.Domain.Enums;

namespace StudyZen.Domain.Entities;

public sealed class FlashcardSet : BaseEntity
{
    public int? LectureId { get; set; }
    public string Name { get; set; }
    public Color Color { get; set; }

    public FlashcardSet(int? lectureId, string name, Color color) : base(default)
    {
        LectureId = lectureId;
        Name = name;
        Color = color;
    }
}