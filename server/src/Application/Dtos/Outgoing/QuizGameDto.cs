using StudyZen.Domain.Enums;

namespace StudyZen.Application.Dtos;

public sealed record QuizGameDto(
    QuizDto Quiz,
    string Pin, 
    QuizGameState State);
