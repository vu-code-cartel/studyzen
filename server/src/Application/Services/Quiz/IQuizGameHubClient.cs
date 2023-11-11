using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface IQuizGameHubClient
{
    Task OnPlayerJoin(QuizPlayerDto player);
    Task OnPlayerLeave(QuizPlayerDto player);
    Task OnQuestionReceive(QuizGameQuestionDto question);
}
