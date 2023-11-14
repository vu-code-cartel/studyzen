namespace StudyZen.Application.Dtos;

public sealed record QuizGameQuestionDto(
    string Question, 
    IReadOnlyCollection<QuizGameChoiceDto> Choices);