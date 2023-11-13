using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Extensions;

public static class QuizAnswerExtensions
{
    public static QuizAnswerDto ToDto(this QuizAnswer answer)
    {
        return new QuizAnswerDto(
            answer.Id,
            answer.Answer,
            answer.IsCorrect);
    }

    public static List<QuizAnswerDto> ToDto(this IEnumerable<QuizAnswer> answers)
    {
        return answers.Select(a => a.ToDto()).ToList();
    }
}
