using StudyZen.FlashCards.Requests;
using StudyZen.Persistence;


namespace StudyZen.FlashCards
{
    
    public interface IFlashcardService
    {
        int AddFlashcard(CreateFlashCardRequest request);
        FlashCard GetFlashcard(int flashcardId);
    }

    public sealed class FlashcardService : IFlashcardService
    {
        private static List<FlashCard> _flashcards = new List<FlashCard>();

        private int _Id = 1;
        
        public int AddFlashcard(CreateFlashCardRequest request)
        {
            var flashcard = new FlashCard(1, request.Question, request.Answer);
            _flashcards.Add(flashcard);
            _Id ++;
            return flashcard.Id;
            
        }
        

        public FlashCard GetFlashcard(int flashcardId)
        {
            return _flashcards.FirstOrDefault(f => f.Id == flashcardId);
        }
    }
    
} 