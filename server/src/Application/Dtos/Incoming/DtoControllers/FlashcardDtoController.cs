using StudyZen.Domain.Entities;
using StudyZen.Application.Dtos;

namespace StudyZen.DtoControllers
{
    public class FlashcardDtoController
    {
        public static FlashcardDto ToDto(Flashcard flashcard)
        {
            return new FlashcardDto
            {
               Id = flashcard.Id,
               FlashcardSetId = flashcard.FlashcardSetId,
               Question = flashcard.Question,
               Answer = flashcard.Answer
            };
        }
    }
}