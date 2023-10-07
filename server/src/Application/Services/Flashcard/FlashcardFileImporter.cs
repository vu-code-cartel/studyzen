using System.Threading;
using StudyZen.Application.Dtos;


namespace StudyZen.Application.Services
{
    public class FlashcardFileImporter
    {
        private readonly IFlashcardService _flashcardService;

        public FlashcardFileImporter(IFlashcardService flashcardService)
        {
            _flashcardService = flashcardService;
        }

       public void ImportFlashcardsFromCsvStream(Stream stream)
        {
            try
            {
                using (var reader = new StreamReader(stream))
                {
                    var flashcardImportThreads = new List<Thread>();

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        var values = line.Split(',');

                        if (values.Length == 3 && int.TryParse(values[0], out int flashcardSetId))
                        {
                            string question = values[1];
                            string answer = values[2];

                            var createFlashcardDto = new CreateFlashcardDto(flashcardSetId, question, answer);

                            var thread = new Thread(() =>
                            {
                                _flashcardService.CreateFlashcard(createFlashcardDto);
                            });

                            flashcardImportThreads.Add(thread);
                            thread.Start();
                        }
                    }

                    foreach (var thread in flashcardImportThreads)
                    {
                        thread.Join();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing flashcards: {ex.Message}");
            }
        }
    }
}
