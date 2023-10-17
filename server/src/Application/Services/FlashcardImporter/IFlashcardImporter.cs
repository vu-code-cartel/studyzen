using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface IFlashcardImporter
{
    IReadOnlyCollection<CreateFlashcardDto> ImportFlashcardsFromCsv(Stream stream, int flashcardSetId);
}