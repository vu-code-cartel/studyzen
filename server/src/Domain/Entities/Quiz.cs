using System.ComponentModel.DataAnnotations;
using StudyZen.Domain.Constraints;

namespace StudyZen.Domain.Entities;

public sealed class Quiz : AuditableEntity
{
    [Required]
    [StringLength(QuizConstraints.TitleMaxLength)]
    public string Title { get; set; }

    public List<QuizQuestion> Questions { get; set; } = new();

    public Quiz(string title)
    {
        Title = title;
    }
}
