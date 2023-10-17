using StudyZen.Application.Dtos;
using StudyZen.Application.ValueObjects;

namespace StudyZen.Application.Services;

public interface IFlashcardImporter
{
    IReadOnlyCollection<CreateFlashcardDto> ImportFlashcardsFromCsv(Stream stream, int flashcardSetId, FileMetadata fileMetadata);
}