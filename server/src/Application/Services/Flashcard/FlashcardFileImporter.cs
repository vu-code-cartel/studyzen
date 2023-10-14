using System.Collections.Concurrent;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services
{
    public class FlashcardImporter
    {
        private readonly IFlashcardService _flashcardService;

        public FlashcardImporter(IFlashcardService flashcardService)
        {
            _flashcardService = flashcardService;
        }

        public IEnumerable<CreateFlashcardDto> ImportFlashcardsFromCsvStream(Stream stream, int flashcardSetId)
        {
            try
            {
                var lines = ReadLinesFromStream(stream);

                var flashcardsToCreate = new BlockingCollection<CreateFlashcardDto>();

                Parallel.ForEach(lines, line =>
                {
                    var values = line.Split(',');

                    if (values.Length == 2)
                    {
                        string front = values[0];
                        string back = values[1];

                        var createFlashcardDto = new CreateFlashcardDto(flashcardSetId, front, back);

                        flashcardsToCreate.Add(createFlashcardDto);
                    }
                });
              
                flashcardsToCreate.CompleteAdding();
             
                return flashcardsToCreate.GetConsumingEnumerable();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing flashcards: {ex.Message}");
                
                return Enumerable.Empty<CreateFlashcardDto>();
            }
        }

        private List<string> ReadLinesFromStream(Stream stream)
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