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
            throw new IdentifiableException(ErrorCodes.QuizGameAlreadyStarted);
        }

        var player = game.Players.Find(u => u.Username == dto.Username);
        if (player is not null)
        {
            throw new IdentifiableException(ErrorCodes.UsernameTaken);
        }

        player = new QuizPlayer(dto.Username, dto.GamePin, dto.ConnectionId);
        _quizGameRepository.AddPlayerToGame(player, game);

        await _hubContext.Clients.Group(dto.GamePin).OnPlayerJoin(player.ToDto());
    }

    public async Task StartGame(string gamePin)
    {
        var game = _quizGameRepository.GetGameChecked(gamePin);

        if (game.State != QuizGameState.NotStarted)
        {
            throw new IdentifiableException(ErrorCodes.QuizGameAlreadyStarted);
        }

        game.State = QuizGameState.InProgress;
        await _hubContext.Clients.Group(gamePin).OnGameStart();
    }

    public void SendNextQuestion(string gamePin)
    {
        var game = _quizGameRepository.GetGameChecked(gamePin);

        if (game.State == QuizGameState.Finished || game.QuestionCancellationSource is not null || !game.Questions.TryDequeue(out var question))
        {
            return;
        }

        game.Players.ForEach(p => p.HasAnswered = false);
        Task.Factory.StartNew(async () =>
        {
            game.CurrentQuestion = question;
            game.QuestionCancellationSource = new CancellationTokenSource();
            var cancellationToken = game.QuestionCancellationSource.Token;

            var dto = new QuizGameQuestionDto(
                question.Question,
                question.Choices.Select(c => new QuizGameChoiceDto(c.Id, c.Answer)).ToList(),
                question.TimeLimit.Seconds);

            var correctAnswerIds = question.Choices.Where(c => c.IsCorrect).Select(c => c.Id);
            var group = _hubContext.Clients.Group(gamePin);
            await group.OnQuestionReceive(dto);

            try
            {
                await Task.Delay(question.TimeLimit, cancellationToken).WaitAsync(cancellationToken);
            }
            catch (OperationCanceledException) {}

            await group.OnAnswerReceive(correctAnswerIds);
            game.QuestionCancellationSource = null;
        });
    }

    public void SubmitAnswer(string gamePin, string connectionId, IList<int> answerIds)
    {
        var game = _quizGameRepository.GetGameChecked(gamePin);
        var player = _quizGameRepository.GetPlayerByConnectionId(connectionId);

        if (game.State == QuizGameState.Finished || game.CurrentQuestion is null || player is null || player.HasAnswered)
        {
            return;
        }

        var isAnswerCorrect = game.CurrentQuestion
            .Choices
            .All(choice => 
                (!choice.IsCorrect || answerIds.Contains(choice.Id)) 
                && (choice.IsCorrect || !answerIds.Contains(choice.Id)));

        if (isAnswerCorrect)
        {
            player.AccumulatedPoints += 1000;
        }

        player.HasAnswered = true;
    }

    public async Task SendScoreboard(string gamePin)
    {
        var game = _quizGameRepository.GetGameChecked(gamePin);
        if (game.QuestionCancellationSource is not null)
        {
            return;
        }

        var players = game.Players.ToDto();
        if (game.State == QuizGameState.Finished || game.Questions.Count <= 0)
        {
            await _hubContext.Clients.Group(gamePin).OnGameFinish(players);
            game.State = QuizGameState.Finished;
        }
        else
        {
            await _hubContext.Clients.Group(gamePin).OnScoreboardReceive(players);
        }
    }

    public bool AreAllPlayersAnswered(string gamePin)
    {
        var game = _quizGameRepository.GetGameChecked(gamePin);
        return game.Players.All(p => p.HasAnswered);
    }

    public void FinishQuestion(string gamePin)
    {
        var game = _quizGameRepository.GetGameChecked(gamePin);
        game.QuestionCancellationSource?.Cancel();
    }
}
