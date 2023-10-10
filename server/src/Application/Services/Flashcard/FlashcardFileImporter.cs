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

       public void ImportFlashcardsFromCsvStream(Stream stream, int flashcardSetId)
        {
            try
            {
                var flashcardsToCreate = new List<CreateFlashcardDto>();
                using (var reader = new StreamReader(stream))
                {

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        var values = line.Split(',');

                        if (values.Length == 2)
                        {
                            string question = values[0];
                            string answer = values[1];

                            var createFlashcardDto = new CreateFlashcardDto(flashcardSetId, question, answer);

                            flashcardsToCreate.Add(createFlashcardDto);

                        }
                    }   
                }
                 _flashcardService.CreateFlashcardsCollection(flashcardsToCreate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing flashcards: {ex.Message}");
            }
        }
    }
}
