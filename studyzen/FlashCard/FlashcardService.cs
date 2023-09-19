using StudyZen.FlashCards.Requests;
using StudyZen.Persistence;
using StudyZen.FlashCardSetClass;


namespace StudyZen.FlashCards
{

    public interface IFlashCardService
    {
        FlashCard AddFlashCard(CreateFlashCardRequest request);
        FlashCard? GetFlashCard(int flashCardId);
        public bool DeleteFlashCard(int flashCardId);
        IReadOnlyCollection<FlashCard> GetFlashCardsBySetId(int? flashCardSetId);
        FlashCard? UpdateFlashCardById(int flashCardId, UpdateFlashCardRequest request);

    }

    public sealed class FlashcardService : IFlashCardService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FlashcardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public FlashCard AddFlashCard(CreateFlashCardRequest request)
        {
            FlashCard flashCard = new FlashCard(request.FlashCardSetId, request.Question, request.Answer);
            _unitOfWork.FlashCards.Add(flashCard);
            return flashCard;

        }

        public FlashCard? GetFlashCard(int flashCardId)
        {
            return _unitOfWork.FlashCards.GetById(flashCardId);
        }

        public bool DeleteFlashCard(int flashCardId)
        {          
             _unitOfWork.FlashCards.Delete(flashCardId);
             
            return true; 
        }

        public FlashCard? UpdateFlashCardById(int flashCardId, UpdateFlashCardRequest request)
    {
        var toBeUpdatedFlashCard = _unitOfWork.FlashCards.GetById(flashCardId);
        if (toBeUpdatedFlashCard  == null)
        {
            return null;
        }
        if (request.Question != null)
        {
            toBeUpdatedFlashCard.Question = request.Question;
        }
        if (request.Answer != null)
        {
            toBeUpdatedFlashCard.Answer = request.Answer;
        }
        _unitOfWork.FlashCards.Update(toBeUpdatedFlashCard);
        
        return toBeUpdatedFlashCard;
    }

        public IReadOnlyCollection<FlashCard> GetFlashCardsBySetId(int? flashCardSetId)
        {
            var allFlashCards = _unitOfWork.FlashCards.GetAll();
            if (flashCardSetId != null)
            {
                var setFlashCards = allFlashCards.Where(flashCard => flashCard.FlashCardSetId == flashCardSetId);
                return setFlashCards.ToList();
            }
            else
            {
                return allFlashCards;
            }
        }
      

    }

}

