using StudyZen.FlashCards.Requests;
using StudyZen.Persistence;
using StudyZen.FlashCardSetClass;


namespace StudyZen.FlashCards
{

    public interface IFlashcardService
    {
        int AddFlashcard(CreateFlashCardRequest request);
        FlashCard GetFlashcard(int flashcardId);
        int AddFlashCardSet(CreateFlashCardSetRequest request); 
        FlashCardSet GetFlashCardSet(int flashCardSetId);
        public bool DeleteFlashCard(int flashcardId);
        public bool DeleteFlashCardSet(int flashCardSetId);
        public  List<FlashCard> GetAllFlashCards();

        public List<FlashCardSet> GetAllFlashCardSets();
        public void UpdateFlashCard(FlashCard flashCard);
        public void UpdateFlashCardSet(FlashCardSet flashCardSet);

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
            FlashCard flashcard = new FlashCard(request.Question, request.Answer);
            _unitOfWork.FlashCards.Add(flashcard);
            return flashcard.Id;

        }

        public List<FlashCard> GetAllFlashCards()
        {
        
            return _unitOfWork.FlashCards.GetAll();

        }

        public List<FlashCardSet> GetAllFlashCardSets()
        {

            return _unitOfWork.FlashCardSets.GetAll();
            
        }

        public int AddFlashCardSet(CreateFlashCardSetRequest request)
    {
    
        FlashCardSet flashCardSet = new FlashCardSet(request.SetName, request.Color, request.LectureId);

    
        _unitOfWork.FlashCardSets.Add(flashCardSet);

        return flashCardSet.Id;
    }


        public FlashCard? GetFlashcard(int flashcardId)
        {
            return _unitOfWork.FlashCards.GetById(flashcardId);
        }

        public FlashCardSet GetFlashCardSet(int flashCardSetId)
        {
             return _unitOfWork.FlashCardSets.GetById(flashCardSetId);
        }

        public bool DeleteFlashCard(int flashcardId)
        {
            var flashcard = GetFlashcard(flashcardId);
            if (flashcard == null)
            {
                return false; 
            }

             _unitOfWork.FlashCards.Delete(flashcardId);
            return true; 
        }

        public bool DeleteFlashCardSet(int flashCardSetId)
        {
            var flashCardSet = GetFlashCardSet(flashCardSetId);
            if (flashCardSet == null)
            {
                return false;
            }
 
             _unitOfWork.FlashCardSets.Delete(flashCardSetId);

            return true; 
        }

        public void UpdateFlashCard(FlashCard flashCard)
        {
            _unitOfWork.FlashCards.Update(flashCard);
        }    

         public void UpdateFlashCardSet(FlashCardSet flashCardSet)
        {
            _unitOfWork.FlashCardSets.Update(flashCardSet);
        }       

      

    }

}

