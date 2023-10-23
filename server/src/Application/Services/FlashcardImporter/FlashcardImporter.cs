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
        var batches = PartitionBatch(lines, 3); 
        var threadList = new List<Thread>();

        foreach (var batch in batches)
        {
            var thread = new Thread(() =>
            {
                foreach (var line in batch)
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

    private IEnumerable<IEnumerable<string>> PartitionBatch(IEnumerable<string> source, int numberOfBatches)
    {
        var batch = new List<string>();
        int batchSize = source.Count() / numberOfBatches;
        int currentBatchSize = 0;

        foreach (var item in source)
        {
            batch.Add(item);
            currentBatchSize++;

            if (currentBatchSize >= batchSize)
            {
                yield return batch;
                batch = new List<string>();
                currentBatchSize = 0;
            }
        }

        if (batch.Count > 0)
        {
            yield return batch;
        }
    }
}