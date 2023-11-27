using Microsoft.AspNetCore.SignalR;
using Moq;
using StudyZen.Application.Dtos;
using StudyZen.Application.Exceptions;
using StudyZen.Application.Extensions;
using StudyZen.Application.Repositories;
using StudyZen.Application.Services;
using StudyZen.Domain.Entities;
using StudyZen.Domain.Enums;
using System.Linq.Expressions;

namespace StudyZen.Application.UnitTests.Services;

public class QuizGameServiceTests
{
    private Mock<IHubContext<QuizGameHub, IQuizGameHubClient>> _hubContext;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IQuizRepository> _quizRepository;
    private Mock<IQuizGameRepository> _gameRepository;
    private Mock<IQuizQuestionRepository> _questionRepository;
    private Mock<IGroupManager> _hubGroup;
    private Mock<IHubClients<IQuizGameHubClient>> _hubClients;
    private Mock<IQuizGameHubClient> _hubClient;
    private IQuizGameService _quizGameService;

    [SetUp]
    public void SetUp()
    {
        _hubContext = new Mock<IHubContext<QuizGameHub, IQuizGameHubClient>>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _quizRepository = new Mock<IQuizRepository>();
        _gameRepository = new Mock<IQuizGameRepository>();
        _questionRepository = new Mock<IQuizQuestionRepository>();
        _hubGroup = new Mock<IGroupManager>();
        _hubClients = new Mock<IHubClients<IQuizGameHubClient>>();
        _hubClient = new Mock<IQuizGameHubClient>();

        _unitOfWork.SetupGet(u => u.QuizQuestions).Returns(_questionRepository.Object);
        _unitOfWork.SetupGet(u => u.Quizzes).Returns(_quizRepository.Object);
        _hubContext.SetupGet(c => c.Groups).Returns(_hubGroup.Object);
        _hubContext.SetupGet(c => c.Clients).Returns(_hubClients.Object);

        _quizGameService = new QuizGameService(_hubContext.Object, _gameRepository.Object, _unitOfWork.Object);
    }

    [Test]
    public async Task CreateGame_ValidParameters_GameCreated()
    {
        var quiz = new Quiz("Quiz_Title")
        {
            Id = 1
        };

        var questions = new List<QuizQuestion>
        {
            new(quiz.Id, "question_1", TimeSpan.FromSeconds(15)),
            new(quiz.Id, "question_2", TimeSpan.FromSeconds(15))
        };

        _unitOfWork.Setup(r => r.Quizzes.GetByIdChecked(quiz.Id)).ReturnsAsync(quiz);
        _unitOfWork.Setup(r => r.QuizQuestions.Get(
            It.IsAny<Expression<Func<QuizQuestion, bool>>>(),
            It.IsAny<Func<IQueryable<QuizQuestion>, IOrderedQueryable<QuizQuestion>>>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<bool>(),
            It.IsAny<Expression<Func<QuizQuestion, object>>[]>())).ReturnsAsync(questions);

        var dto = await _quizGameService.CreateGame(quiz.Id);

        _gameRepository.Verify(r => r.AddGame(It.IsAny<QuizGame>()), Times.Once);

        Assert.That(dto.Quiz.Id, Is.EqualTo(quiz.Id));
        Assert.That(dto.Quiz.Title, Is.EqualTo(quiz.Title));
        Assert.That(dto.State, Is.EqualTo(QuizGameState.NotStarted));
    }

    [Test]
    public void GetGame_GameExists_ReturnsGame()
    {
        var expectedGame = new QuizGame(1, Guid.NewGuid().ToString(), Array.Empty<QuizQuestion>())
        {
            Pin = Guid.NewGuid().ToString()
        };

        _gameRepository.Setup(r => r.GetGameChecked(expectedGame.Pin)).Returns(expectedGame);

        var actualGame = _quizGameService.GetGame(expectedGame.Pin);

        Assert.That(actualGame.Pin, Is.EqualTo(expectedGame.Pin));
        Assert.That(actualGame.Quiz.Title, Is.EqualTo(expectedGame.QuizTitle));
    }

    [Test]
    public async Task ConnectToGame_GameExists_UserAddedToGame()
    {
        var connectionId = Guid.NewGuid().ToString();
        var game = new QuizGame(1, Guid.NewGuid().ToString(), Array.Empty<QuizQuestion>())
        {
            Pin = Guid.NewGuid().ToString()
        };

        _gameRepository.Setup(r => r.GetGameChecked(game.Pin)).Returns(game);

        await _quizGameService.ConnectToGame(game.Pin, connectionId);

        _hubContext.Verify(c => c.Groups.AddToGroupAsync(connectionId, game.Pin, default), Times.Once);
    }

    [Test]
    public async Task Quit_PlayerDoesNotExist_Returns()
    {
        var connectionId = Guid.NewGuid().ToString();

        await _quizGameService.Quit(connectionId);

        _gameRepository.Verify(r => r.GetPlayerByConnectionId(connectionId), Times.Once);
    }

    [Test]
    public async Task Quit_GameDoesNotExist_Returns()
    {
        var player = new QuizPlayer(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        var connectionId = Guid.NewGuid().ToString();

        _gameRepository.Setup(r => r.GetPlayerByConnectionId(connectionId)).Returns(player);

        await _quizGameService.Quit(connectionId);

        _gameRepository.Verify(r => r.GetPlayerByConnectionId(connectionId), Times.Once);
        _gameRepository.Verify(r => r.GetGame(player.GamePin), Times.Once);
    }

    [Test]
    public async Task Quit_ValidUserAndGame_RemovesUserFromGame()
    {
        var player = new QuizPlayer(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        var connectionId = Guid.NewGuid().ToString();
        var game = new QuizGame(1, Guid.NewGuid().ToString(), Array.Empty<QuizQuestion>())
        {
            Pin = Guid.NewGuid().ToString()
        };

        _gameRepository.Setup(r => r.GetPlayerByConnectionId(connectionId)).Returns(player);
        _gameRepository.Setup(r => r.GetGame(player.GamePin)).Returns(game);
        _hubClients.Setup(c => c.Group(game.Pin)).Returns(_hubClient.Object);

        await _quizGameService.Quit(connectionId);

        _gameRepository.Verify(r => r.GetPlayerByConnectionId(connectionId), Times.Once);
        _gameRepository.Verify(r => r.GetGame(player.GamePin), Times.Once);
        _gameRepository.Verify(r => r.RemovePlayerFromGame(player, game), Times.Once);
        _hubGroup.Verify(g => g.RemoveFromGroupAsync(connectionId, game.Pin, default), Times.Once);
        _hubClient.Verify(c => c.OnPlayerLeave(It.IsAny<QuizPlayerDto>()), Times.Once);
    }

    [Test]
    public void Quit_GameExists_PlayersReturned()
    {
        var game = new QuizGame(1, Guid.NewGuid().ToString(), Array.Empty<QuizQuestion>())
        {
            Pin = Guid.NewGuid().ToString(),
            Players = new List<QuizPlayer>()
            {
                new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
            }
        };

        _gameRepository.Setup(r => r.GetGameChecked(game.Pin)).Returns(game);

        var players = _quizGameService.GetPlayers(game.Pin);

        CollectionAssert.AreEquivalent(game.Players.ToDto(), players);
    }

    [Test]
    public void JoinGame_GameIsStarted_Throws()
    {
        var game = new QuizGame(1, Guid.NewGuid().ToString(), Array.Empty<QuizQuestion>())
        {
            Pin = Guid.NewGuid().ToString(),
            State = QuizGameState.InProgress
        };

        _gameRepository.Setup(r => r.GetGameChecked(game.Pin)).Returns(game);

        var dto = new JoinQuizGameDto(game.Pin, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        Assert.ThrowsAsync<IdentifiableException>(async () => await _quizGameService.JoinGame(dto));
    }

    [Test]
    public void JoinGame_UsernameTaken_Throws()
    {
        var gamePin = Guid.NewGuid().ToString();
        var game = new QuizGame(1, Guid.NewGuid().ToString(), Array.Empty<QuizQuestion>())
        {
            Pin = gamePin,
            Players = new List<QuizPlayer>
            {
                new("some_user", gamePin, Guid.NewGuid().ToString())
            }
        };

        _gameRepository.Setup(r => r.GetGameChecked(game.Pin)).Returns(game);

        var dto = new JoinQuizGameDto(game.Pin, "some_user", Guid.NewGuid().ToString());

        Assert.ThrowsAsync<IdentifiableException>(async () => await _quizGameService.JoinGame(dto));
    }

    [Test]
    public async Task JoinGame_ValidArguments_PlayerAddedToGame()
    {
        var gamePin = Guid.NewGuid().ToString();
        var game = new QuizGame(1, Guid.NewGuid().ToString(), Array.Empty<QuizQuestion>())
        {
            Pin = gamePin,
            Players = new List<QuizPlayer>
            {
                new(Guid.NewGuid().ToString(), gamePin, Guid.NewGuid().ToString())
            }
        };

        _gameRepository.Setup(r => r.GetGameChecked(game.Pin)).Returns(game);
        _hubClients.Setup(c => c.Group(game.Pin)).Returns(_hubClient.Object);

        var dto = new JoinQuizGameDto(game.Pin, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        await _quizGameService.JoinGame(dto);

        _gameRepository.Verify(r => r.AddPlayerToGame(It.IsAny<QuizPlayer>(), game), Times.Once);
        _hubClient.Verify(c => c.OnPlayerJoin(It.IsAny<QuizPlayerDto>()), Times.Once);
    }

    [Test]
    public void StartGame_GameAlreadyStarted_Throws()
    {
        var gamePin = Guid.NewGuid().ToString();
        var game = new QuizGame(1, Guid.NewGuid().ToString(), Array.Empty<QuizQuestion>())
        {
            Pin = gamePin,
            Players = new List<QuizPlayer>
            {
                new(Guid.NewGuid().ToString(), gamePin, Guid.NewGuid().ToString())
            },
            State = QuizGameState.InProgress
        };

        _gameRepository.Setup(r => r.GetGameChecked(game.Pin)).Returns(game);

        Assert.ThrowsAsync<IdentifiableException>(async () => await _quizGameService.StartGame(game.Pin));
    }

    [Test]
    public async Task StartGame_ValidParameters_StartsGame()
    {
        var gamePin = Guid.NewGuid().ToString();
        var game = new QuizGame(1, Guid.NewGuid().ToString(), Array.Empty<QuizQuestion>())
        {
            Pin = gamePin,
            Players = new List<QuizPlayer>
            {
                new(Guid.NewGuid().ToString(), gamePin, Guid.NewGuid().ToString())
            },
            State = QuizGameState.NotStarted
        };

        _gameRepository.Setup(r => r.GetGameChecked(game.Pin)).Returns(game);
        _hubClients.Setup(c => c.Group(game.Pin)).Returns(_hubClient.Object);

        await _quizGameService.StartGame(game.Pin);

        _hubClient.Verify(c => c.OnGameStart(), Times.Once);
    }

    [Test]
    public void SendNextQuestion_GameFinished_Returns()
    {
        var gamePin = Guid.NewGuid().ToString();
        var game = new QuizGame(1, Guid.NewGuid().ToString(), Array.Empty<QuizQuestion>())
        {
            Pin = gamePin,
            Players = new List<QuizPlayer>
            {
                new(Guid.NewGuid().ToString(), gamePin, Guid.NewGuid().ToString())
            },
            State = QuizGameState.Finished
        };
        _gameRepository.Setup(r => r.GetGameChecked(game.Pin)).Returns(game);

        _quizGameService.SendNextQuestion(game.Pin);
    }

    [Test]
    public void SendNextQuestion_ValidParameters_SendsQuestion()
    {
        var quizId = 123;
        var gamePin = Guid.NewGuid().ToString();
        var questions = new[]
        {
            new QuizQuestion(quizId, Guid.NewGuid().ToString(), TimeSpan.FromMilliseconds(0)),
            new QuizQuestion(quizId, Guid.NewGuid().ToString(), TimeSpan.FromMilliseconds(0))
        };
        var game = new QuizGame(quizId, Guid.NewGuid().ToString(), questions)
        {
            Pin = gamePin,
            Players = new List<QuizPlayer>
            {
                new(Guid.NewGuid().ToString(), gamePin, Guid.NewGuid().ToString())
            },
            State = QuizGameState.InProgress
        };

        _gameRepository.Setup(r => r.GetGameChecked(game.Pin)).Returns(game);
        _hubClients.Setup(c => c.Group(game.Pin)).Returns(_hubClient.Object);

        _quizGameService.SendNextQuestion(game.Pin);

        // wait a bit for question to get in
        Task.Delay(100).Wait();

        _hubClient.Verify(c => c.OnQuestionReceive(It.IsAny<QuizGameQuestionDto>()));
    }

    [Test]
    public void SubmitAnswer_GameFinished_Returns()
    {
        var gamePin = Guid.NewGuid().ToString();
        var quizId = 1;
        var game = new QuizGame(quizId, Guid.NewGuid().ToString(), Array.Empty<QuizQuestion>())
        {
            Pin = gamePin,
            Players = new List<QuizPlayer>
            {
                new(Guid.NewGuid().ToString(), gamePin, Guid.NewGuid().ToString())
            },
            State = QuizGameState.Finished,
            CurrentQuestion = new QuizQuestion(quizId, Guid.NewGuid().ToString(), TimeSpan.FromSeconds(15))
        };
        var player = new QuizPlayer(Guid.NewGuid().ToString(), game.Pin, Guid.NewGuid().ToString());
        var answers = new List<int> { 1, 2 };

        _gameRepository.Setup(r => r.GetGameChecked(game.Pin)).Returns(game);
        _gameRepository.Setup(r => r.GetPlayerByConnectionId(player.ConnectionId)).Returns(player);

        _quizGameService.SubmitAnswer(game.Pin, player.ConnectionId, answers);
    }

    [TestCase(1000, 1, 3)]
    [TestCase(0, 1)]
    [TestCase(0, 3)]
    [TestCase(0, 1, 2)]
    [TestCase(0, 2)]
    [TestCase(0)]
    public void SubmitAnswer_ValidParameters_AddsPointsIfCorrect(int expectedPoints, params int[] choiceIds)
    {
        var gamePin = Guid.NewGuid().ToString();
        var quizId = 1;
        var game = new QuizGame(quizId, Guid.NewGuid().ToString(), Array.Empty<QuizQuestion>())
        {
            Pin = gamePin,
            Players = new List<QuizPlayer>
            {
                new(Guid.NewGuid().ToString(), gamePin, Guid.NewGuid().ToString())
            },
            State = QuizGameState.InProgress,
            CurrentQuestion = new QuizQuestion(quizId, Guid.NewGuid().ToString(), TimeSpan.FromSeconds(15))
            {
                Id = 1,
                Choices = new List<QuizAnswer>
                {
                    new(1, Guid.NewGuid().ToString(), true)
                    {
                        Id = 1
                    },
                    new(1, Guid.NewGuid().ToString(), false)
                    {
                        Id = 2
                    },
                    new(1, Guid.NewGuid().ToString(), true)
                    {
                        Id = 3
                    }
                }
            }
        };
        var player = new QuizPlayer(Guid.NewGuid().ToString(), game.Pin, Guid.NewGuid().ToString());

        _gameRepository.Setup(r => r.GetGameChecked(game.Pin)).Returns(game);
        _gameRepository.Setup(r => r.GetPlayerByConnectionId(player.ConnectionId)).Returns(player);

        _quizGameService.SubmitAnswer(game.Pin, player.ConnectionId, choiceIds);

        Assert.That(player.AccumulatedPoints, Is.EqualTo(expectedPoints));
        Assert.That(player.HasAnswered, Is.EqualTo(true));
    }

    [Test]
    public async Task SendScoreboard_QuestionIsActive_Returns()
    {
        var quizId = 123;
        var gamePin = Guid.NewGuid().ToString();
        var game = new QuizGame(quizId, Guid.NewGuid().ToString(), Array.Empty<QuizQuestion>())
        {
            Pin = gamePin,
            Players = new List<QuizPlayer>
            {
                new(Guid.NewGuid().ToString(), gamePin, Guid.NewGuid().ToString())
            },
            State = QuizGameState.InProgress,
            QuestionCancellationSource = new CancellationTokenSource()
        };

        _gameRepository.Setup(r => r.GetGameChecked(game.Pin)).Returns(game);

        await _quizGameService.SendScoreboard(game.Pin);
    }

    [Test]
    public async Task SendScoreboard_NoQuestionsLeft_CallsOnGameFinished()
    {
        var quizId = 123;
        var gamePin = Guid.NewGuid().ToString();
        var game = new QuizGame(quizId, Guid.NewGuid().ToString(), Array.Empty<QuizQuestion>())
        {
            Pin = gamePin,
            Players = new List<QuizPlayer>
            {
                new(Guid.NewGuid().ToString(), gamePin, Guid.NewGuid().ToString())
            },
            State = QuizGameState.InProgress
        };

        _gameRepository.Setup(r => r.GetGameChecked(game.Pin)).Returns(game);
        _hubClients.Setup(c => c.Group(game.Pin)).Returns(_hubClient.Object);

        await _quizGameService.SendScoreboard(game.Pin);

        _hubClient.Verify(c => c.OnGameFinish(It.IsAny<IEnumerable<QuizPlayerDto>>()), Times.Once);
        Assert.That(game.State, Is.EqualTo(QuizGameState.Finished));
    }

    [Test]
    public async Task SendScoreboard_QuestionsLeft_SendsScoreboard()
    {
        var quizId = 123;
        var gamePin = Guid.NewGuid().ToString();
        var questions = new[]
        {
            new QuizQuestion(quizId, Guid.NewGuid().ToString(), TimeSpan.FromMilliseconds(0)),
            new QuizQuestion(quizId, Guid.NewGuid().ToString(), TimeSpan.FromMilliseconds(0))
        };
        var game = new QuizGame(quizId, Guid.NewGuid().ToString(), questions)
        {
            Pin = gamePin,
            Players = new List<QuizPlayer>
            {
                new(Guid.NewGuid().ToString(), gamePin, Guid.NewGuid().ToString())
            },
            State = QuizGameState.InProgress
        };

        _gameRepository.Setup(r => r.GetGameChecked(game.Pin)).Returns(game);
        _hubClients.Setup(c => c.Group(game.Pin)).Returns(_hubClient.Object);

        await _quizGameService.SendScoreboard(game.Pin);

        _hubClient.Verify(c => c.OnScoreboardReceive(It.IsAny<IEnumerable<QuizPlayerDto>>()), Times.Once);
    }

    [TestCase(true, true, true, true)]
    [TestCase(false, true, false, true)]
    public void AreAllPlayersAnswered_ValidParameters_ReturnsCorrectAnswer(bool expected, params bool[] hasAnswered)
    {
        var gamePin = Guid.NewGuid().ToString();
        var players = hasAnswered.Select(x => new QuizPlayer(Guid.NewGuid().ToString(), gamePin, Guid.NewGuid().ToString()) { HasAnswered = x }).ToList();
        var game = new QuizGame(1, Guid.NewGuid().ToString(), Array.Empty<QuizQuestion>())
        {
            Pin = gamePin,
            Players = players,
            State = QuizGameState.InProgress
        };

        _gameRepository.Setup(r => r.GetGameChecked(game.Pin)).Returns(game);

        var result = _quizGameService.AreAllPlayersAnswered(game.Pin);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void FinishQuestion_ValidParameters_FinishesQuestion()
    {
        var quizId = 123;
        var gamePin = Guid.NewGuid().ToString();
        var game = new QuizGame(quizId, Guid.NewGuid().ToString(), Array.Empty<QuizQuestion>())
        {
            Pin = gamePin,
            Players = new List<QuizPlayer>
            {
                new(Guid.NewGuid().ToString(), gamePin, Guid.NewGuid().ToString())
            },
            State = QuizGameState.InProgress,
            QuestionCancellationSource = new CancellationTokenSource()
        };

        _gameRepository.Setup(r => r.GetGameChecked(game.Pin)).Returns(game);

        _quizGameService.FinishQuestion(game.Pin);

        Assert.That(game.QuestionCancellationSource.IsCancellationRequested, Is.EqualTo(true));
    }
}