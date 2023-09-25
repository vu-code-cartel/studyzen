using StudyZen.Flashcards.Requests;
using StudyZen.Persistence;

namespace StudyZen.Flashcards;

public interface IFlashcardService
{
    Flashcard CreateFlashcard(CreateFlashcardRequest request);
    Flashcard? GetFlashcardById(int flashcardId);
    IReadOnlyCollection<Flashcard> GetFlashcardsBySetId(int flashcardSetId);
    Flashcard? UpdateFlashcard(int flashcardId, UpdateFlashcardRequest request);
    void DeleteFlashcard(int flashcardId);
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

    public IReadOnlyCollection<Flashcard> GetFlashcardsBySetId(int flashcardSetId)
    {
        var allFlashcards = _unitOfWork.Flashcards.GetAll();
        var setFlashcards = allFlashcards.Where(flashcard => flashcard.FlashcardSetId == flashcardSetId).ToList();
        return setFlashcards;
    }

    public Flashcard? UpdateFlashcard(int flashcardId, UpdateFlashcardRequest request)
    {
        var flashcard = _unitOfWork.Flashcards.GetById(flashcardId);
        if (flashcard is null)
        {
            return null;
        }

        flashcard.Question = request.Question ?? flashcard.Question;
        flashcard.Answer = request.Answer ?? flashcard.Answer;
        _unitOfWork.Flashcards.Update(flashcard);

        return flashcard;
    }

    public void DeleteFlashcard(int flashcardId)
    {
        _unitOfWork.Flashcards.Delete(flashcardId);
    }
}