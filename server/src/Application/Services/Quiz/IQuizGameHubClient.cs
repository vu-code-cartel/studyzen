using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface IQuizGameHubClient
{
    Task OnPlayerJoin(QuizPlayerDto player);
    Task OnPlayerLeave(QuizPlayerDto player);
    Task OnGameStart();
    Task OnGameFinish(IEnumerable<QuizPlayerDto> players);
    Task OnQuestionReceive(QuizGameQuestionDto question);
    Task OnAnswerReceive(IEnumerable<int> answerIds);
    Task OnScoreboardReceive(IEnumerable<QuizPlayerDto> players);
}
