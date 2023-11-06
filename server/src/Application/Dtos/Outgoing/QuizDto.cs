using StudyZen.Domain.Entities;

namespace StudyZen.Application.Dtos;

public sealed record QuizDto
{
    public int Id { get; init; }
    public string Title { get; init; }

    public QuizDto(Quiz quiz)
    {
        Id = quiz.Id;
        Title = quiz.Title;
    }
}