using StudyZen.Api.Exceptions;
using StudyZen.Application.Services; 
using StudyZen.Application.Dtos;


namespace StudyZen.Api.Extensions
{
    public static class FormFileExtensions
    {
        public static IEnumerable<CreateFlashcardDto> ImportFlashcardsFromCsvStream(this IFormFile file, IFlashcardImporter importer, int flashcardSetId)
        {
            IEnumerable<CreateFlashcardDto> flashcards = null;

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    flashcards = importer.ImportFlashcardsFromCsvStream(stream, flashcardSetId);

                    if (flashcards is null)
                    {
                        throw new ImportFailedException("Error importing flashcards: An error occurred during import.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while importing: " + ex.Message);
            }

            return flashcards;
        }
    }
}