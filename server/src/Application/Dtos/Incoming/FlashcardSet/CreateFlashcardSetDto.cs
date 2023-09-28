using StudyZen.Domain.Enums;

namespace StudyZen.Application.Dtos;

public sealed record CreateFlashcardSetDto(int? LectureId, string Name, Color Color);