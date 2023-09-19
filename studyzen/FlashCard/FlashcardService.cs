using StudyZen.FlashCards.Requests;
using StudyZen.Persistence;
using StudyZen.FlashCardSetClass;


namespace StudyZen.FlashCards
{

    public interface IFlashCardService
    {
        FlashCard AddFlashCard(CreateFlashCardRequest request);
        FlashCard GetFlashCard(int flashCardId);
        public bool DeleteFlashCard(int flashCardId);
        IReadOnlyCollection<FlashCard> GetFlashCardsBySetId(int? flashCardSetId);
        public void UpdateFlashCard(FlashCard flashCard);

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
            var flashcard = GetFlashCard(flashCardId);
            if (flashcard == null)
            {
                return false; 
            }

             _unitOfWork.FlashCards.Delete(flashCardId);
            return true; 
        }

        public void UpdateFlashCard(FlashCard flashCard)
        {
            _unitOfWork.FlashCards.Update(flashCard);
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

