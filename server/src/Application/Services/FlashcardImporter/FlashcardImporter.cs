using StudyZen.Application.Dtos;
using StudyZen.Application.Extensions;
using StudyZen.Application.Validation;

namespace StudyZen.Application.Services;

public sealed class FlashcardImporter : IFlashcardImporter
{
    private readonly ValidationHandler _validationHandler;
    private readonly IDataImporter _dataImporter;

    public FlashcardImporter(ValidationHandler validationHandler, IDataImporter dataImporter)
    {
        _validationHandler = validationHandler;
        _dataImporter = dataImporter;
    }

    public async Task<IReadOnlyCollection<CreateFlashcardDto>> ImportFlashcardsFromCsv(
        Stream stream,
        int flashcardSetId,
        FileMetadata fileMetadata)
    {
        await _validationHandler.ValidateAsync(fileMetadata);

        return _dataImporter.Import(
            stream.ReadLines(),
            (string front, string back) => new CreateFlashcardDto(flashcardSetId, front, back)).ToList();
    }
}