using StudyZen.Domain.Entities;

namespace StudyZen.Application.Dtos;

public sealed record QuizAnswerDto
{
    public int Id { get; init; }
    public string Answer { get; init; }

    public QuizAnswerDto(QuizAnswer answer)
    {
        Id = answer.Id;
        Answer = answer.Answer;
    }
}
