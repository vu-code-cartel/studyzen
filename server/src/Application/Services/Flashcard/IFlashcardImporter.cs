using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface IFlashcardImporter
{
    IEnumerable<CreateFlashcardDto> ImportFlashcardsFromCsvStream(Stream stream, int flashcardSetId);
    List<string> ReadLinesFromStream(Stream stream);

}