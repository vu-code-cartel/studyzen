using StudyZen.Flashcards.Requests;
using StudyZen.Persistence;


namespace StudyZen.Flashcards
{
    public interface IFlashcardService
    {
        Flashcard CreateFlashcard(CreateFlashcardRequest request);
        Flashcard? GetFlashcardById(int flashcardId);
        IReadOnlyCollection<Flashcard> GetFlashcardsBySetId(int flashcardSetId);
        IReadOnlyCollection<Flashcard> GetAllFlashcards();
        Flashcard? UpdateFlashcardById(int flashcardId, UpdateFlashcardRequest request);
        void DeleteFlashcardById(int flashcardId);
    }

    public sealed class FlashcardService : IFlashcardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FlashcardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Flashcard CreateFlashcard(CreateFlashcardRequest request)
        {
            var newFlashcard = new Flashcard(request.FlashcardSetId, request.Question, request.Answer);
            _unitOfWork.Flashcards.Add(newFlashcard);
            return newFlashcard;
        }

        public Flashcard? GetFlashcardById(int flashcardId)
        {
            return _unitOfWork.Flashcards.GetById(flashcardId);
        }

        public IReadOnlyCollection<Flashcard> GetAllFlashcards()
        {
            return _unitOfWork.Flashcards.GetAll();
        }

        public IReadOnlyCollection<Flashcard> GetFlashcardsBySetId(int flashcardSetId)
        {
            var allFlashcards = _unitOfWork.Flashcards.GetAll();
            return allFlashcards.Where(flashcard => flashcard.FlashcardSetId == flashcardSetId).ToList();
        }

        public Flashcard? UpdateFlashcardById(int flashcardId, UpdateFlashcardRequest request)
        {
            var toBeUpdatedFlashcard = _unitOfWork.Flashcards.GetById(flashcardId);
            if (toBeUpdatedFlashcard == null)
            {
                return null;
            }
            if (request.Question != null)
            {
                toBeUpdatedFlashcard.Question = request.Question;
            }
            if (request.Answer != null)
            {
                toBeUpdatedFlashcard.Answer = request.Answer;
            }
            _unitOfWork.Flashcards.Update(toBeUpdatedFlashcard);
            return toBeUpdatedFlashcard;
        }

        public void DeleteFlashcardById(int flashcardId)
        {
            _unitOfWork.Flashcards.Delete(flashcardId);
        }

    }
}