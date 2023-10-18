using StudyZen.Domain.Enums;
using StudyZen.Domain.Constraints;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudyZen.Domain.Entities;

public sealed class FlashcardSet : BaseEntity
{
    public int? LectureId { get; set; }

    [ForeignKey(nameof(LectureId))]
    public Lecture? Lecture { get; set; } = null;

    [Required]
    [StringLength(FlashcardSetConstraints.NameMaxLength)]
    public string Name { get; set; }

    [Required]
    public Color Color { get; set; }

    public List<Flashcard> Flashcards { get; set; } = new();

    public FlashcardSet(int? lectureId, string name, Color color) : base(default)
    {
        LectureId = lectureId;
        Name = name;
        Color = color;
    }
}