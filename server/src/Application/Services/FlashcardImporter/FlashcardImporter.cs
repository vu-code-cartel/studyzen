using FluentValidation;
using StudyZen.Application.Dtos;
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

    public IReadOnlyCollection<CreateFlashcardDto> ImportFlashcardsFromCsv(Stream stream, int flashcardSetId, FileMetadata fileMetadata)
    {
        _validationHandler.Validate(fileMetadata);

        var lines = stream.ReadLines();
        var importedFlashcards = new BlockingCollection<CreateFlashcardDto>();

        Parallel.ForEach(lines, line =>
        {
            var values = line.Split(',');
            if (values.Length != 2)
            {
                throw new ValidationException($"CSV file must only contain flashcard front and back values. Invalid values: {string.Join(',', values)}");
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