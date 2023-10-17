namespace StudyZen.Application.Dtos;

public sealed record FlashcardDto(
    int Id,
    int FlashcardSetId,
    string Front,
    string Back);