using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Extensions;

public static class QuizExtensions
{
    public static QuizDto ToDto(this Quiz quiz)
    {
        return new QuizDto(quiz.Id, quiz.Title);
    }

    public static List<QuizDto> ToDto(this IEnumerable<Quiz> quizzes)
    {
        return quizzes.Select(q => q.ToDto()).ToList();
    }
}
