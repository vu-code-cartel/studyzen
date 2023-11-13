using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface IQuizGameService
{
    Task<QuizGameDto> CreateGame(int quizId);
    QuizGameDto GetGame(string gamePin);
    Task ConnectToGame(string gamePin, string connectionId);
    Task Quit(string connectionId);
    Task JoinGame(JoinQuizGameDto dto);
    List<QuizPlayerDto> GetPlayers(string gamePin);
    void StartGame(string gamePin);
    Task SendNextQuestion(string gamePin);
}
