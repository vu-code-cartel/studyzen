using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface IFlashcardImporter
{
    Task<IReadOnlyCollection<CreateFlashcardDto>> ImportFlashcardsFromCsv(
        Stream stream,
        int flashcardSetId,
        FileMetadata fileMetadata);
}