using StudyZen.FlashcardSets.Requests;
using StudyZen.Persistence;
using StudyZen.FlashcardSetClass;
using StudyZen.Common;


namespace StudyZen.FlashcardSets
{
    public interface IFlashcardSetService
    {
        FlashcardSet AddFlashcardSet(CreateFlashcardSetRequest request);
        FlashcardSet? GetFlashcardSetById(int flashcardSetId);
        IReadOnlyCollection<FlashcardSet> GetAllFlashcardSets();
        IReadOnlyCollection<FlashcardSet> GetFlashcardSetsByLectureId(int? lectureId);
        FlashcardSet? UpdateFlashcardSetById(int flachCardSetId, UpdateFlashcardSetRequest request);
        void DeleteFlashcardSetById(int flashcardSetId);
    }

    public sealed class FlashcardSetService : IFlashcardSetService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FlashcardSetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public FlashcardSet AddFlashcardSet(CreateFlashcardSetRequest? request)
        {
            request = request.ThrowIfRequestArgumentNull(nameof(request));
            FlashcardSet flashcardSet = new FlashcardSet(request.LectureId, request.Name, request.Color);
            _unitOfWork.FlashcardSets.Add(flashcardSet);
            return flashcardSet;
        }

        public FlashcardSet? GetFlashcardSetById(int flashcardSetId)
        {
            return _unitOfWork.FlashcardSets.GetById(flashcardSetId);
        }

        public IReadOnlyCollection<FlashcardSet> GetAllFlashcardSets()
        {
            return _unitOfWork.FlashcardSets.GetAll();
        }

        public IReadOnlyCollection<FlashcardSet> GetFlashcardSetsByLectureId(int? lectureId)
        {
            var allFlashcardSets = _unitOfWork.FlashcardSets.GetAll();
            return allFlashcardSets.Where(flashcardset => flashcardset.LectureId == lectureId).ToList();
        }

        public FlashcardSet? UpdateFlashcardSetById(int flachCardSetId, UpdateFlashcardSetRequest request)
        {
            var toBeUpdatedFlashcardSet = _unitOfWork.FlashcardSets.GetById(flachCardSetId);
            if (toBeUpdatedFlashcardSet == null)
            {
                return null;
            }
            if (request.Name != null)
            {
                toBeUpdatedFlashcardSet.Name = request.Name;
            }
            if (request.Color != null)
            {
                toBeUpdatedFlashcardSet.Color = request.Color.Value;
            }
            else
            {
                toBeUpdatedFlashcardSet.Color = Color.None;
            }
            toBeUpdatedFlashcardSet.LectureId = request.LectureId;
            _unitOfWork.FlashcardSets.Update(toBeUpdatedFlashcardSet);
            return toBeUpdatedFlashcardSet;
        }
        public void DeleteFlashcardSetById(int flashcardSetId)
        {
            _unitOfWork.FlashcardSets.Delete(flashcardSetId);
        }

    }
}