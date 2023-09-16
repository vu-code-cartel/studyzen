using StudyZen.FlashCards.Requests;
using StudyZen.Persistence;
using StudyZen.FlashCardSetClass;



namespace StudyZen.FlashCards
{
    
    public interface IFlashcardService
    {
        int AddFlashcard(CreateFlashCardRequest request);
        FlashCard GetFlashcard(int flashcardId);
        int CreateFlashcardSet(string setName,  FlashCardSetColor color);
        void AddFlashcardToSet(int setId, int flashcardId);
        FlashCardSet GetFlashcardSet(int setId);
    }

    public sealed class FlashcardService :IFlashcardService
    {
        private static List<FlashCard> _flashcards = new List<FlashCard>();
        private static List<FlashCardSet> _flashcardSets = new List<FlashCardSet>();

        private int _Id = 1;
        private int _setId = 1;
        
        public int AddFlashcard(CreateFlashCardRequest request)
        {
            var flashcard = new FlashCard(_Id, request.Question, request.Answer);
            _flashcards.Add(flashcard);
            _Id ++;
            return flashcard.Id;
            
        }

        public int CreateFlashcardSet(string setName, FlashCardSetColor color)
        {
            var set = new FlashCardSet(_setId, setName, color);
            _flashcardSets.Add(set);
            _setId++;
            return set.Id;
        }
        

        public FlashCard GetFlashcard(int flashcardId)
        {
            return _flashcards.FirstOrDefault(f => f.Id == flashcardId);
        }

        public void AddFlashcardToSet(int setId, int flashcardId)
        {
             var set = _flashcardSets.FirstOrDefault(s => s.Id == setId);
             if (set != null)
             {
                set.FlashCardIds.Add(flashcardId);
             }
        }

        public FlashCardSet GetFlashcardSet(int setId)
        {
            return _flashcardSets.FirstOrDefault(s => s.Id == setId);
        }

    }

    
} 