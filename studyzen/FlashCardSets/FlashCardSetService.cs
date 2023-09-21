using StudyZen.FlashCardSets.Requests;
using StudyZen.Persistence;
using StudyZen.FlashCardSetClass;
using StudyZen.Common;


namespace StudyZen.FlashCardSets
{
    public interface IFlashCardSetService
    {
        FlashCardSet AddFlashCardSet(CreateFlashCardSetRequest request);
        FlashCardSet? GetFlashCardSetById(int flashCardSetId);
        IReadOnlyCollection<FlashCardSet> GetAllFlashCardSets();
        IReadOnlyCollection<FlashCardSet> GetFlashCardSetsByLectureId(int? lectureId);
        FlashCardSet? UpdateFlashCardSetById(int flachCardSetId, UpdateFlashCardSetRequest request);
        void DeleteFlashCardSetById(int flashCardSetId);
    }

    public sealed class FlashCardSetService : IFlashCardSetService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FlashCardSetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public FlashCardSet AddFlashCardSet(CreateFlashCardSetRequest? request)
        {
            request = request.ThrowIfRequestArgumentNull(nameof(request));
            FlashCardSet flashCardSet = new FlashCardSet(request.LectureId, request.Name, request.Color);
            _unitOfWork.FlashCardSets.Add(flashCardSet);
            return flashCardSet;
        }

        public FlashCardSet? GetFlashCardSetById(int flashCardSetId)
        {
            return _unitOfWork.FlashCardSets.GetById(flashCardSetId);
        }

        public IReadOnlyCollection<FlashCardSet> GetAllFlashCardSets()
        {
            return _unitOfWork.FlashCardSets.GetAll();
        }

        public IReadOnlyCollection<FlashCardSet> GetFlashCardSetsByLectureId(int? lectureId)
        {
            var allFlashCardSets = _unitOfWork.FlashCardSets.GetAll();
            return allFlashCardSets.Where(flashcardset => flashcardset.LectureId == lectureId).ToList();
        }

        public FlashCardSet? UpdateFlashCardSetById(int flachCardSetId, UpdateFlashCardSetRequest request)
        {
            var toBeUpdatedFlashCardSet = _unitOfWork.FlashCardSets.GetById(flachCardSetId);
            if (toBeUpdatedFlashCardSet == null)
            {
                return null;
            }
            if (request.Name != null)
            {
                toBeUpdatedFlashCardSet.Name = request.Name;
            }
            if (request.Color != null)
            {
                toBeUpdatedFlashCardSet.Color = request.Color.Value;
            }
            else
            {
                toBeUpdatedFlashCardSet.Color = Color.None;
            }
            toBeUpdatedFlashCardSet.LectureId = request.LectureId;
            _unitOfWork.FlashCardSets.Update(toBeUpdatedFlashCardSet);
            return toBeUpdatedFlashCardSet;
        }
        public void DeleteFlashCardSetById(int flashCardSetId)
        {
            _unitOfWork.FlashCardSets.Delete(flashCardSetId);
        }

    }
}