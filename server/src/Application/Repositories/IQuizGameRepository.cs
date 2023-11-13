using StudyZen.Domain.Entities;

namespace StudyZen.Application.Repositories;

public interface IQuizGameRepository
{
    void AddGame(QuizGame game);
    QuizGame? GetGame(string gamePin);
    QuizGame GetGameChecked(string gamePin);
    void AddPlayerToGame(QuizPlayer player, QuizGame game);
    QuizPlayer? GetPlayerByConnectionId(string connectionId);
    void RemovePlayerFromGame(QuizPlayer player, QuizGame game);
}
