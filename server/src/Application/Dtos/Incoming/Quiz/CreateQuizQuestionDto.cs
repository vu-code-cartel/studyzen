namespace StudyZen.Application.Dtos;

public sealed record CreateQuizQuestionDto(
    string Question,
    IEnumerable<string> CorrectAnswers,
    IEnumerable<string> IncorrectAnswers,
    int TimeLimitInSeconds);