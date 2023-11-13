using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Extensions;

public static class QuizQuestionExtensions
{
    public static QuizQuestionDto ToDto(this QuizQuestion question)
    {
        return new QuizQuestionDto(
            question.Id,
            question.Question,
            question.Choices.ToDto(),
            question.TimeLimit.Seconds);
    }

    public static List<QuizQuestionDto> ToDto(this IEnumerable<QuizQuestion> questions)
    {
        return questions.Select(q => q.ToDto()).ToList();
    }
}
