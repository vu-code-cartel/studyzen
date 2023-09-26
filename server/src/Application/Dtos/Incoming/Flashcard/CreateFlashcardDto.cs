namespace StudyZen.Application.Dtos;

public sealed record CreateFlashcardDto(int FlashcardSetId, string Question, string Answer);