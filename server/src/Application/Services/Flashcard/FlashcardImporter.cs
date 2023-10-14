using System.Collections.Concurrent;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services
{
    public sealed class FlashcardImporter : IFlashcardImporter
    {
        public IEnumerable<CreateFlashcardDto> ImportFlashcardsFromCsvStream(Stream stream, int flashcardSetId)
        {
            
            var lines = ReadLinesFromStream(stream);         

            try
            {
                var flashcardsToCreate = new BlockingCollection<CreateFlashcardDto>();
                int successfullyAddedCount = 0;

                Parallel.ForEach(lines, line =>
                {
                    var values = line.Split(',');

                    if (values.Length == 2)
                    {
                        string front = values[0];
                        string back = values[1];

                        var createFlashcardDto = new CreateFlashcardDto(flashcardSetId, front, back);

                        if (flashcardsToCreate.TryAdd(createFlashcardDto))
                        {
                            successfullyAddedCount++;
                        }
                    }
                });
                
                flashcardsToCreate.CompleteAdding();

                if (successfullyAddedCount != lines.Count)
                {
                    return null;
                }

                return flashcardsToCreate.GetConsumingEnumerable();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while importing: " + ex.Message);
            }
        }

        public List<string> ReadLinesFromStream(Stream stream)
        {
            var lines = new List<string>();
            using var reader = new StreamReader(stream);
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        lines.Add(line);
                    }
                }
            }
            return lines;
        }
    }
}