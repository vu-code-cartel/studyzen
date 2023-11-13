namespace StudyZen.Domain.Entities;

public sealed class QuizPlayer
{
    public string ConnectionId { get; set; }
    public string Username { get; set; }
    public string GamePin { get; set; }
    public int AccumulatedPoints { get; set; }

    public QuizPlayer(string username, string gamePin, string connectionId)
    {
        Username = username;
        GamePin = gamePin;
        ConnectionId = connectionId;
        AccumulatedPoints = 0;
    }
}