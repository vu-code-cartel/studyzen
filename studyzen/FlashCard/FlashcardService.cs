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

        public int AddFlashCardSet(CreateFlashCardSetRequest request)
    {
    
        FlashCardSet flashCardSet = new FlashCardSet(request.SetName, request.Color, request.LectureId);

    
        _unitOfWork.FlashCardSets.Add(flashCardSet);

    
        foreach (var flashCardRequest in request.FlashCards)
        {
            int flashcardId = AddFlashcard(flashCardRequest);
            flashCardSet.FlashCardIds.Add(flashcardId);
        }

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

   
            foreach (var flashcardId in flashCardSet.FlashCardIds)
            {
                _unitOfWork.FlashCards.Delete(flashcardId);
            }

 
             _unitOfWork.FlashCardSets.Delete(flashCardSetId);

            return true; 
        }

      

    }

}

