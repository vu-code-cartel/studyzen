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
    Task StartGame(string gamePin);
    void SendNextQuestion(string gamePin);
    void SubmitAnswer(string gamePin, string connectionId, IList<int> answerIds);
    Task SendScoreboard(string gamePin);
    bool AreAllPlayersAnswered(string gamePin);
    void FinishQuestion(string gamePin);
}
