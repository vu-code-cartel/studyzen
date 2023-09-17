using StudyZen.FlashCards.Requests;
using StudyZen.Persistence;
using StudyZen.FlashCardSetClass;



namespace StudyZen.FlashCards
{
    
    public interface IFlashcardService
    {
        int AddFlashcard(CreateFlashCardRequest request);
        FlashCard GetFlashcard(int flashcardId);
        int CreateFlashcardSet(string setName,  FlashCardSetColor color, int? lectureId);
        void AddFlashcardToSet(int setId, int flashcardId);
        FlashCardSet GetFlashcardSet(int setId);
        List<FlashCard> GetFlashcardsInSet(int setId);
        bool DeleteFlashcardSet(int setId);
        bool DeleteFlashCard(int flashcardId);
    }


    public sealed class FlashcardService :IFlashcardService
    {
        private static List<FlashCard> _flashcards = new List<FlashCard>();
       
        private static List<FlashCardSet> _flashcardSets = new List<FlashCardSet>();
        
        public int AddFlashcard(CreateFlashCardRequest request)
        {
            var flashcard = new FlashCard(_flashcards.Count + 1, request.Question, request.Answer);
            _flashcards.Add(flashcard);
            return flashcard.Id;
            
        }

        public int CreateFlashcardSet(string setName, FlashCardSetColor color, int? lectureId)
        {
            var set = new FlashCardSet(_flashcardSets.Count + 1, setName, color, lectureId);
            _flashcardSets.Add(set);
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

        public List<FlashCard> GetFlashcardsInSet(int setId)
        {
            var set = _flashcardSets.FirstOrDefault(s => s.Id == setId);
            if (set != null)
            {
                return _flashcards.Where(f => set.FlashCardIds.Contains(f.Id)).ToList();
            }
            return new List<FlashCard>();
        }

     /*   public bool DeleteFlashcardSet(int setId)
        {
            var flashCardSetToRemove = _flashcardSets.FirstOrDefault(s => s.Id == setId);
            if (flashCardSetToRemove  != null)
            {
                _flashcardSets.Remove(flashCardSetToRemove );
                return true;
            }
                return false;
        }

        public bool DeleteFlashCard(int flashcardId)
        {
            var flashcardToRemove = _flashcards.FirstOrDefault(f => f.Id == flashcardId);
            if (flashcardToRemove != null )
            {
                _flashcards.Remove(flashcardToRemove);
                return true;
            }
                return false;
        }*/
        public bool DeleteFlashcardSet(int setId)
    {
        return DeleteItem(setId, _flashcardSets, fs => fs.Id);
    }

    public bool DeleteFlashCard(int flashcardId)
    {
        return DeleteItem(flashcardId, _flashcards, fc => fc.Id);
    }

       public bool DeleteItem<T>(int itemId, List<T> itemList, Func<T, int> getIdFunc)
    {
        var itemToRemove = itemList.FirstOrDefault(item => getIdFunc(item) == itemId);
        if (itemToRemove != null)
        {
            itemList.Remove(itemToRemove);
            return true;
        }
        return false;
    }



    }

    
} 