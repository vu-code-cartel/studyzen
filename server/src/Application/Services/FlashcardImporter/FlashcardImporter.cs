using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Extensions;
using StudyZen.Application.Validation;
using System.Collections.Concurrent;
using StudyZen.Application.ValueObjects;

namespace StudyZen.Application.Services;

public sealed class FlashcardImporter : IFlashcardImporter
{
    private readonly ValidationHandler _validationHandler;

    public FlashcardImporter(ValidationHandler validationHandler)
    {
        _validationHandler = validationHandler;
    }

    public async Task<IReadOnlyCollection<CreateFlashcardDto>> ImportFlashcardsFromCsv(Stream stream, int flashcardSetId, FileMetadata fileMetadata)
    {
        await _validationHandler.ValidateAsync(fileMetadata);

        var lines = stream.ReadLines();
        var importedFlashcards = new BlockingCollection<CreateFlashcardDto>();
        var threadList = new List<Thread>();
        var lockObject = new object(); 

        foreach (var line in lines)
        {
            var localLine = line; 

            var thread = new Thread(() =>
            {
                var values = localLine.Split(',');
                if (values.Length != 2)
                {
                    throw new ValidationException($"CSV file must only contain flashcard front and back values. Invalid values: {string.Join(',', values)}");
                }

                var front = values[0];
                var back = values[1];
                var createFlashcardDto = new CreateFlashcardDto(flashcardSetId, front, back);

                lock (lockObject)
                {
                    importedFlashcards.Add(createFlashcardDto);
                }
            });

            threadList.Add(thread);
            thread.Start();
        }

        foreach (var thread in threadList)
        {
            thread.Join();
        }

        importedFlashcards.CompleteAdding();

        return importedFlashcards.GetConsumingEnumerable().ToList();
    }
}