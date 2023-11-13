using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Extensions;

public static class QuizGameExtensions
{
    public static QuizGameDto ToDto(this QuizGame game)
    {
        return new QuizGameDto(
            new QuizDto(game.QuizId, game.QuizTitle), 
            game.Pin, 
            game.State);
    }
}
