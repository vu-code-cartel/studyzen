namespace StudyZen.Application.Dtos;

public sealed record CreateFlashcardDto(int FlashcardSetId, string Front, string Back);