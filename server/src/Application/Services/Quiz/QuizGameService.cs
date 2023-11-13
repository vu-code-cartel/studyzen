using Microsoft.AspNetCore.SignalR;
using StudyZen.Application.Dtos;
using StudyZen.Application.Exceptions;
using StudyZen.Application.Extensions;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Domain.Enums;

namespace StudyZen.Application.Services;

public sealed class QuizGameService : IQuizGameService
{
    private readonly IHubContext<QuizGameHub, IQuizGameHubClient> _hubContext;
    private readonly IQuizGameRepository _quizGameRepository;
    private readonly IUnitOfWork _unitOfWork;

    public QuizGameService(
        IHubContext<QuizGameHub, IQuizGameHubClient> hubContext, 
        IQuizGameRepository quizGameRepository, 
        IUnitOfWork unitOfWork)
    {
        _hubContext = hubContext;
        _quizGameRepository = quizGameRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<QuizGameDto> CreateGame(int quizId)
    {
        var quiz = await _unitOfWork.Quizzes.GetByIdChecked(quizId);
        var questions = await _unitOfWork.QuizQuestions.Get(
            q => q.QuizId == quiz.Id, 
            includes: q => q.Choices);

        var game = new QuizGame(quiz.Id, quiz.Title, questions);

        _quizGameRepository.AddGame(game);

        return game.ToDto();
    }

    public QuizGameDto GetGame(string gamePin)
    {
        return _quizGameRepository.GetGameChecked(gamePin).ToDto();
    }

    public async Task ConnectToGame(string gamePin, string connectionId)
    {
        _quizGameRepository.GetGameChecked(gamePin);
        await _hubContext.Groups.AddToGroupAsync(connectionId, gamePin);
    }

    public async Task Quit(string connectionId)
    {
        var player = _quizGameRepository.GetPlayerByConnectionId(connectionId);
        if (player is null)
        {
            return;
        }

        var game = _quizGameRepository.GetGame(player.GamePin);
        if (game is null)
        {
            return;
        }

        _quizGameRepository.RemovePlayerFromGame(player, game);
        await _hubContext.Groups.RemoveFromGroupAsync(connectionId, game.Pin);
        await _hubContext.Clients.Group(game.Pin).OnPlayerLeave(player.ToDto());
    }

    public List<QuizPlayerDto> GetPlayers(string gamePin)
    {
        var game = _quizGameRepository.GetGameChecked(gamePin);
        return game.Players.ToDto();
    }

    public async Task JoinGame(JoinQuizGameDto dto)
    {
        var game = _quizGameRepository.GetGameChecked(dto.GamePin);

        if (game.State != QuizGameState.NotStarted)
        {
            throw new QuizGameAlreadyStartedException();
        }

        var player = game.Players.Find(u => u.Username == dto.Username);
        if (player is not null)
        {
            throw new UsernameTakenException();
        }

        player = new QuizPlayer(dto.Username, dto.GamePin, dto.ConnectionId);
        _quizGameRepository.AddPlayerToGame(player, game);

        await _hubContext.Clients.Group(dto.GamePin).OnPlayerJoin(player.ToDto());
    }

    public void StartGame(string gamePin)
    {
        var game = _quizGameRepository.GetGameChecked(gamePin);

        if (game.State != QuizGameState.NotStarted)
        {
            throw new QuizGameAlreadyStartedException();
        }

        game.State = QuizGameState.InProgress;
    }

    public async Task SendNextQuestion(string gamePin)
    {
        // TODO: fix this next week

        var game = _quizGameRepository.GetGameChecked(gamePin);

        if (!game.Questions.TryDequeue(out var question))
        {
            return; // TODO: quiz finished
        }

        var dto = new QuizGameQuestionDto(
            question.Question,
            question.Choices.Select(c => new QuizGameChoiceDto(c.Id, c.Answer)).ToList());

        await _hubContext.Clients.Group(gamePin).OnQuestionReceive(dto);
    }
}
