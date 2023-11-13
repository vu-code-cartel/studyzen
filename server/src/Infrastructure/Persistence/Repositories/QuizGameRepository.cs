using StudyZen.Application.Exceptions;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public sealed class QuizGameRepository : IQuizGameRepository
{
    private readonly Random _random = new();
    private readonly Dictionary<string, QuizGame> _gameByPin = new();
    private readonly Dictionary<string, QuizPlayer> _playerByConnectionId = new();

    public void AddGame(QuizGame game)
    {
        string gameId;
        var it = 0;

        while (true)
        {
            if (++it > 10)
            {
                // unlucky you, try next time
                throw new Exception("Too many conflicts while generating quiz game PIN.");
            }

            const string chars = "0123456789";
            const int length = 5;
            gameId = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());

            if (!_gameByPin.ContainsKey(gameId))
            {
                break;
            }
        }

        game.Pin = gameId;
        _gameByPin[gameId] = game;
    }

    public QuizGame? GetGame(string gamePin)
    {
        return _gameByPin.TryGetValue(gamePin, out var game) ? game : null;
    }

    public QuizGame GetGameChecked(string gamePin)
    {
        return GetGame(gamePin) ?? throw new QuizGameNotFoundException();
    }

    public void AddPlayerToGame(QuizPlayer player, QuizGame game)
    {
        game.Players.Add(player);
        _playerByConnectionId[player.ConnectionId] = player;
    }

    public QuizPlayer? GetPlayerByConnectionId(string connectionId)
    {
        return _playerByConnectionId.TryGetValue(connectionId, out var player) ? player : null;
    }

    public void RemovePlayerFromGame(QuizPlayer player, QuizGame game)
    {
        game.Players.Remove(player);
        _playerByConnectionId.Remove(player.ConnectionId);
    }
}
