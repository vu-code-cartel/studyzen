using StudyZen.FlashCards.Requests;
using StudyZen.Persistence;
using StudyZen.FlashCardSetClass;


namespace StudyZen.FlashCards
{

    public interface IFlashCardService
    {
        int AddFlashCard(CreateFlashCardRequest request);
        FlashCard GetFlashCard(int flashCardId);
        public bool DeleteFlashCard(int flashCardId);
        public  List<FlashCard> GetAllFlashCards();
        public void UpdateFlashCard(FlashCard flashCard);

    }

    public sealed class FlashcardService : IFlashCardService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FlashcardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int AddFlashCard(CreateFlashCardRequest request)
        {
            FlashCard flashCard = new FlashCard(request.Question, request.Answer);
            _unitOfWork.FlashCards.Add(flashCard);
            return flashCard.Id;

        }

        public List<FlashCard> GetAllFlashCards()
        {
        
            return _unitOfWork.FlashCards.GetAll();

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
      

    }

}

