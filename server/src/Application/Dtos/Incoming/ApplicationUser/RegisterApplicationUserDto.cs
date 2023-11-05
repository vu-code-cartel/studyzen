namespace StudyZen.Application.Dtos;

public sealed record RegisterApplicationUserDto(string Username, string Email, string Password, string FirstName, string LastName, string Role);