using StudyZen.Application.Dtos;
using StudyZen.Application.Exceptions;
using StudyZen.Application.Extensions;
using StudyZen.Application.Validation;
using System.Collections.Concurrent;

namespace StudyZen.Application.Services;

public sealed class FlashcardImporter : IFlashcardImporter
{
    private readonly ValidationHandler _validationHandler;

    public FlashcardImporter(ValidationHandler validationHandler)
    {
        _validationHandler = validationHandler;
    }

    public async Task<IReadOnlyCollection<CreateFlashcardDto>> ImportFlashcardsFromCsv(
        Stream stream,
        int flashcardSetId,
        FileMetadata fileMetadata)
    {
        await _validationHandler.ValidateAsync(fileMetadata);

        var lines = stream.ReadLines();

        try
        {
            return ImportFlashcardsInParallel(lines, flashcardSetId);
        }
        catch (AggregateException ex) when (ex.InnerException is IncorrectArgumentCountException)
        {
            throw ex.InnerException;
        }
    }

    private IReadOnlyCollection<CreateFlashcardDto> ImportFlashcardsInParallel(
        IEnumerable<string> lines,
        int flashcardSetId)
    {
        var importedFlashcards = new BlockingCollection<CreateFlashcardDto>();

        Parallel.ForEach(lines, line =>
        {
            var values = line.Split(',');
            if (values.Length != 2)
            {
                throw new IncorrectArgumentCountException("FileContent");
            }

            var front = values[0];
            var back = values[1];
            var createFlashcardDto = new CreateFlashcardDto(flashcardSetId, front, back);

            importedFlashcards.Add(createFlashcardDto);
        });

        importedFlashcards.CompleteAdding();

        return importedFlashcards.GetConsumingEnumerable().ToList();
    }
}