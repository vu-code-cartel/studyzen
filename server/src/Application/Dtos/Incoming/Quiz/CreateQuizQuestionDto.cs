namespace StudyZen.Application.Dtos;

public sealed record CreateQuizQuestionDto(
    string Question,
    string CorrectAnswer,
    IEnumerable<string> IncorrectAnswers,
    int TimeLimitInSeconds);