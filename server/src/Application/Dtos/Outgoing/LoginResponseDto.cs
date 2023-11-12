namespace StudyZen.Application.Dtos;

public sealed record LoginResponseDto(string AccessToken, string RefreshToken);