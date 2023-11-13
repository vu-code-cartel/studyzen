namespace StudyZen.Application.Dtos;

public sealed record QuizQuestionDto(
    int Id, 
    string Question, 
    List<QuizAnswerDto> Choices, 
    int TimeLimitInSeconds);