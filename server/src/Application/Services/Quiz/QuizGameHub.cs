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

    public Task<List<QuizPlayerDto>> GetPlayers(string gamePin)
    {
        return Task.FromResult(_quizGameService.GetPlayers(gamePin));
    }
}
