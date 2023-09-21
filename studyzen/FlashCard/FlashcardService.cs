using StudyZen.FlashCards.Requests;
using StudyZen.Persistence;
using StudyZen.FlashCardSetClass;


namespace StudyZen.FlashCards
{

    public interface IFlashCardService
    {
        FlashCard AddFlashCard(CreateFlashCardRequest request);
        FlashCard? GetFlashCardById(int flashCardId);
        void DeleteFlashCardById(int flashCardId);
        IReadOnlyCollection<FlashCard> GetFlashCardsBySetId(int? flashCardSetId);
        FlashCard? UpdateFlashCardById(int flashCardId, UpdateFlashCardRequest request);
        public IReadOnlyCollection<FlashCard> GetAllFlashCards();
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
            var newFlashCard = new FlashCard(request.FlashCardSetId, request.Question, request.Answer);
            _unitOfWork.FlashCards.Add(newFlashCard);
            return newFlashCard;

        }

        public FlashCard? GetFlashCardById(int flashCardId)
        {
            return _unitOfWork.FlashCards.GetById(flashCardId);
        }

        public void DeleteFlashCardById(int flashCardId)
        {
            _unitOfWork.FlashCards.Delete(flashCardId);
        }

        public FlashCard? UpdateFlashCardById(int flashCardId, UpdateFlashCardRequest request)
        {
            var toBeUpdatedFlashCard = _unitOfWork.FlashCards.GetById(flashCardId);
            if (toBeUpdatedFlashCard == null)
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
            return allFlashCards.Where(flashCard => flashCard.FlashCardSetId == flashCardSetId).ToList();
        }

        public IReadOnlyCollection<FlashCard> GetAllFlashCards()
        {
            return _unitOfWork.FlashCards.GetAll();
        }

    }

}

