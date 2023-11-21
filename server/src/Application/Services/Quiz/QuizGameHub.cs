using Microsoft.AspNetCore.SignalR;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public sealed class QuizGameHub : Hub<IQuizGameHubClient>
{
    private readonly IQuizGameService _quizGameService;

    public QuizGameHub(IQuizGameService quizGameService)
    {
        _quizGameService = quizGameService;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _quizGameService.Quit(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task ConnectToGame(string gamePin)
    {
        await _quizGameService.ConnectToGame(gamePin, Context.ConnectionId);
    }

    public List<QuizPlayerDto> GetPlayers(string gamePin)
    {
        return _quizGameService.GetPlayers(gamePin);
    }

    public void SubmitAnswer(string gamePin, IList<int> answerIds)
    {
        _quizGameService.SubmitAnswer(gamePin, Context.ConnectionId, answerIds);

        if (_quizGameService.AreAllPlayersAnswered(gamePin))
        {
            _quizGameService.FinishQuestion(gamePin);
        }
    }

    public async Task SendScoreboard(string gamePin)
    {
        await _quizGameService.SendScoreboard(gamePin);
    }

    public void NextQuestion(string gamePin)
    {
        _quizGameService.SendNextQuestion(gamePin);
    }
}
