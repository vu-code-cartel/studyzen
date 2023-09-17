using StudyZen.FlashCards.Requests;
using StudyZen.Persistence;
using StudyZen.FlashCardSetClass;



namespace StudyZen.FlashCards
{

    public interface IFlashcardService
    {
        int AddFlashcard(CreateFlashCardRequest request);
        FlashCard GetFlashcard(int flashcardId);
        // int CreateFlashcardSet(string setName, FlashCardSetColor color, int? lectureId);
        // void AddFlashcardToSet(int setId, int flashcardId);
        // FlashCardSet GetFlashcardSet(int setId);
        // List<FlashCard> GetFlashcardsInSet(int setId);
        // bool DeleteFlashcardSet(int setId);
        // bool DeleteFlashCard(int flashcardId);
    }


    public sealed class FlashcardService : IFlashcardService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FlashcardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int AddFlashcard(CreateFlashCardRequest request)
        {
            FlashCard flashcard = new FlashCard(request.SetId, request.Question, request.Answer);
            _unitOfWork.FlashCards.Add(flashcard);
            return flashcard.Id;

        }
        // public int CreateFlashcardSet(string setName, FlashCardSetColor color, int? lectureId)
        // {
        //     var set = new FlashCardSet(_flashcardSets.Count + 1, setName, color, lectureId);
        //     _flashcardSets.Add(set);
        //     return set.Id;
        // }


        public FlashCard? GetFlashcard(int flashcardId)
        {
            return _unitOfWork.FlashCards.GetById(flashcardId);
        }

        // public void AddFlashcardToSet(int setId, int flashcardId)
        // {
        //     var set = _flashcardSets.FirstOrDefault(s => s.Id == setId);
        //     if (set != null)
        //     {
        //         set.FlashCardIds.Add(flashcardId);
        //     }
        // }

        // public FlashCardSet GetFlashcardSet(int setId)
        // {
        //     return _flashcardSets.FirstOrDefault(s => s.Id == setId);
        // }

        // public List<FlashCard> GetFlashcardsInSet(int setId)
        // {
        //     var set = _flashcardSets.FirstOrDefault(s => s.Id == setId);
        //     if (set != null)
        //     {
        //         return _flashcards.Where(f => set.FlashCardIds.Contains(f.Id)).ToList();
        //     }
        //     return new List<FlashCard>();
        // }


        // public bool DeleteFlashcardSet(int setId)
        // {
        //     return DeleteItem(setId, _flashcardSets, fs => fs.Id);
        // }

        // public bool DeleteFlashCard(int flashcardId)
        // {
        //     return DeleteItem(flashcardId, _flashcards, fc => fc.Id);
        // }

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