using StudyZen.Application.Dtos;
using StudyZen.Application.ValueObjects;

namespace StudyZen.Application.Services;

public interface IFlashcardImporter
{
    Task<IReadOnlyCollection<CreateFlashcardDto>> ImportFlashcardsFromCsv(Stream stream, int flashcardSetId, FileMetadata fileMetadata);
}