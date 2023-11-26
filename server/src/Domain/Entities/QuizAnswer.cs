using StudyZen.Domain.Constraints;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace StudyZen.Domain.Entities;

[ExcludeFromCodeCoverage]
public sealed class QuizAnswer : BaseEntity
{
    [Required]
    [ForeignKey(nameof(QuizQuestion))]
    public int QuizQuestionId { get; set; }

    public QuizQuestion QuizQuestion { get; set; } = null!;

    [Required]
    [StringLength(QuizConstraints.AnswerMaxLength)]
    public string Answer { get; set; }

    [Required]
    public bool IsCorrect { get; set; }

    public QuizAnswer(int quizQuestionId, string answer, bool isCorrect)
    {
        Answer = answer;
        QuizQuestionId = quizQuestionId;
        IsCorrect = isCorrect;
    }
}
