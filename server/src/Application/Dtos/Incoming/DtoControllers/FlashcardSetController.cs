using StudyZen.Domain.Entities;
using StudyZen.Application.Dtos;

namespace StudyZen.DtoControllers
{
    public class FlashcardSetDtoController
    {
        public static FlashcardSetDto ToDto(FlashcardSet flashcardSet)
        {
            return new FlashcardSetDto
            {
                LectureId = flashcardSet.LectureId,
                Name = flashcardSet.Name,
                Color = flashcardSet.Color
            };
        }
    }
}
