using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Extensions;

public static class QuizPlayerExtensions
{
    public static QuizPlayerDto ToDto(this QuizPlayer user)
    {
        return new QuizPlayerDto(user.Username, user.AccumulatedPoints);
    }

    public static List<QuizPlayerDto> ToDto(this IEnumerable<QuizPlayer> players)
    {
        return players.Select(p => p.ToDto()).ToList();
    }
}
