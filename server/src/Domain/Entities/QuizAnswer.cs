using StudyZen.Domain.Constraints;
using System.ComponentModel.DataAnnotations;

namespace StudyZen.Domain.Entities;

public sealed class QuizAnswer : BaseEntity
{
    [Required]
    public int QuizQuestionId { get; set; }

    [Required]
    [StringLength(QuizConstraints.AnswerMaxLength)]
    public string Answer { get; set; }

    public QuizAnswer(int quizQuestionId, string answer)
    {
        Answer = answer;
        QuizQuestionId = quizQuestionId;
    }
}
