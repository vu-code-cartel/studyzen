using StudyZen.FlashcardSets.Requests;
using StudyZen.Persistence;

namespace StudyZen.FlashcardSets;

public interface IFlashcardSetService
{
    FlashcardSet CreateFlashcardSet(CreateFlashcardSetRequest request);
    FlashcardSet? GetFlashcardSetById(int flashcardSetId);
    IReadOnlyCollection<FlashcardSet> GetAllFlashcardSets();
    IReadOnlyCollection<FlashcardSet> GetFlashcardSetsByLectureId(int? lectureId);
    FlashcardSet? UpdateFlashcardSet(int flashCardSetId, UpdateFlashcardSetRequest request);
    void DeleteFlashcardSet(int flashcardSetId);
}

public sealed class FlashcardSetService : IFlashcardSetService
{
    private readonly IUnitOfWork _unitOfWork;

    public FlashcardSetService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public FlashcardSet CreateFlashcardSet(CreateFlashcardSetRequest request)
    {
        var newFlashcardSet = new FlashcardSet(request.LectureId, request.Name, request.Color);
        _unitOfWork.FlashcardSets.Add(newFlashcardSet);
        return newFlashcardSet;
    }

    public FlashcardSet? GetFlashcardSetById(int flashcardSetId)
    {
        var flashcardSet = _unitOfWork.FlashcardSets.GetById(flashcardSetId);
        return flashcardSet;
    }

    public IReadOnlyCollection<FlashcardSet> GetAllFlashcardSets()
    {
        var allFlashcardSets = _unitOfWork.FlashcardSets.GetAll();
        return allFlashcardSets;
    }

    public IReadOnlyCollection<FlashcardSet> GetFlashcardSetsByLectureId(int? lectureId)
    {
        var allFlashcardSets = _unitOfWork.FlashcardSets.GetAll();
        var lectureFlashcardSets = allFlashcardSets.Where(s => s.LectureId == lectureId).ToList();
        return lectureFlashcardSets;
    }

    public FlashcardSet? UpdateFlashcardSet(int flashCardSetId, UpdateFlashcardSetRequest request)
    {
        var flashcardSet = _unitOfWork.FlashcardSets.GetById(flashCardSetId);
        if (flashcardSet is null)
        {
            return null;
        }

        flashcardSet.Name = request.Name ?? flashcardSet.Name;
        flashcardSet.Color = request.Color ?? flashcardSet.Color;
        _unitOfWork.FlashcardSets.Update(flashcardSet);

        return flashcardSet;
    }

    public void DeleteFlashcardSet(int flashcardSetId)
    {
        DeleteFlashcardsBySetId(flashcardSetId);
        _unitOfWork.FlashcardSets.Delete(flashcardSetId);
    }

    private void DeleteFlashcardsBySetId(int flashcardSetId)
    {
        var allFlashcards = _unitOfWork.Flashcards.GetAll();
        var setFlashcards = allFlashcards.Where(flashcard => flashcardSetId == flashcard.FlashcardSetId);

        foreach (var setFlashcard in setFlashcards)
        {
            _unitOfWork.Flashcards.Delete(setFlashcard.Id);
        }
    }
}