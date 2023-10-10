using Microsoft.AspNetCore.Http;
using StudyZen.Api.Exceptions;
using StudyZen.Application.Services; 

namespace StudyZen.Api.Extensions
{
    public static class FormFileExtensions
    {
        public static void ImportFlashcardsFromCsvStream(this IFormFile file, FlashcardFileImporter importer)
        {
            if (file == null)
            {
                throw new RequestArgumentNullException(nameof(file));
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    importer.ImportFlashcardsFromCsvStream(stream);
                }
            }
            catch (Exception ex)
            {
                throw new ImportFailedException("Error importing flashcards.", ex);
            }
        }
    }
}
