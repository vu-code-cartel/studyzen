using StudyZen.Domain.Enums;

namespace StudyZen.Application.Dtos;

public sealed record UpdateFlashcardSetDto(string? Name, Color? Color);