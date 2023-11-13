using StudyZen.Domain.Enums;

namespace StudyZen.Domain.Entities;

public sealed class QuizGame
{
    public string Pin { get; set; }
    public int QuizId { get; set; }
    public string QuizTitle { get; set; }
    public QuizGameState State { get; set; }
    public List<QuizPlayer> Players { get; set; }
    public Queue<QuizQuestion> Questions { get; set; }

    public QuizGame(
        int quizId,
        string quizTitle,
        IEnumerable<QuizQuestion> questions)
    {
        Pin = default!;
        QuizId = quizId;
        QuizTitle = quizTitle;
        Players = new List<QuizPlayer>();
        Questions = new Queue<QuizQuestion>(questions);
        State = QuizGameState.NotStarted;
    }
}