namespace StudyZen.Application.Dtos;

public sealed record JoinQuizGameDto(
    string GamePin, 
    string Username,
    string ConnectionId);