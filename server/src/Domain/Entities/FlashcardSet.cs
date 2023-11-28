using StudyZen.Domain.Enums;
using StudyZen.Domain.Constraints;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudyZen.Domain.Entities;

public sealed class FlashcardSet : AuditableEntity
{
    public int? LectureId { get; set; }

    [ForeignKey(nameof(LectureId))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public Lecture? Lecture { get; set; } = null;

    [Required]
    [StringLength(FlashcardSetConstraints.NameMaxLength)]
    public string Name { get; set; }

    [Required]
    public Color Color { get; set; }

    public List<Flashcard> Flashcards { get; set; } = new();

    public FlashcardSet(int? lectureId, string name, Color color)
    {
        LectureId = lectureId;
        Name = name;
        Color = color;
    }
}