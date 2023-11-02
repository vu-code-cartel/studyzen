using StudyZen.Domain.Entities;

namespace StudyZen.Application.Dtos;

public sealed record QuizQuestionDto
{
    public int Id { get; init; }
    public string Question { get; init; }
    public List<QuizAnswerDto> PossibleAnswers { get; init; }
    public int? CorrectAnswerId { get; init; }
    public int TimeLimitInSeconds { get; init; }

    public QuizQuestionDto(QuizQuestion quizQuestion)
    {
        Id = quizQuestion.Id;
        Question = quizQuestion.Question;
        PossibleAnswers = quizQuestion.PossibleAnswers.Select(i => new QuizAnswerDto(i)).ToList();
        CorrectAnswerId = quizQuestion.CorrectAnswerId;
        TimeLimitInSeconds = quizQuestion.TimeLimit.Seconds;
    }
}