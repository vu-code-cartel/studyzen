using Microsoft.AspNetCore.Http;
using StudyZen.Api.Exceptions;
using StudyZen.Application.Services; 

namespace StudyZen.Api.Extensions
{
    public static class FormFileExtensions
    {
        public static void ImportFlashcardsFromCsvStream(this IFormFile file, FlashcardImporter importer, int flashcardSetId)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    importer.ImportFlashcardsFromCsvStream(stream, flashcardSetId);
                }
            }
            catch (ImportFailedException)
            {
                throw; 
            }
        }
    }
}
