using StudyZen.Domain.Constraints;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyZen.Domain.Entities;

public sealed class QuizQuestion : BaseEntity
{
    [Required]
    [StringLength(QuizConstraints.QuestionMaxLength)]
    public string Question { get; set; }

    [Required]
    public int QuizId { get; set; }

    [Required]
    [ForeignKey(nameof(QuizId))]
    public Quiz Quiz { get; set; } = null!;

    public int? CorrectAnswerId { get; set; }

    public QuizAnswer? CorrectAnswer { get; set; } = null!;

    public List<QuizAnswer> PossibleAnswers { get; set; } = new();

    [Required]
    public TimeSpan TimeLimit { get; set; }

    public QuizQuestion(int quizId, string question, TimeSpan timeLimit)
    {
        QuizId = quizId;
        Question = question;
        TimeLimit = timeLimit;
    }
}
