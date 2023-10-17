using StudyZen.Domain.Enums;

namespace StudyZen.Application.Dtos;

public sealed record FlashcardSetDto(
    int Id,
    int? LectureId,
    string Name,
    Color Color);