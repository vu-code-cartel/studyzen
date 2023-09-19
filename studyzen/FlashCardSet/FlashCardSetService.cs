using StudyZen.FlashCards.Requests;
using StudyZen.Persistence;
using StudyZen.FlashCardSetClass;


namespace StudyZen.FlashCardSets
{

    public interface IFlashCardSetService
    {
        FlashCardSet AddFlashCardSet(CreateFlashCardSetRequest request); 
        FlashCardSet? GetFlashCardSet(int flashCardSetId);
        public bool DeleteFlashCardSet(int flashCardSetId);
        IReadOnlyCollection<FlashCardSet> GetFlashCardSetsByLectureId(int? lectureId);

    }

    public sealed class FlashCardSetService : IFlashCardSetService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FlashCardSetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public FlashCardSet AddFlashCardSet(CreateFlashCardSetRequest request)
        {
    
            FlashCardSet flashCardSet = new FlashCardSet(request.SetName, request.Color, request.LectureId);

            _unitOfWork.FlashCardSets.Add(flashCardSet);

            return flashCardSet;
        }



        public FlashCardSet? GetFlashCardSet(int flashCardSetId)
        {
             return _unitOfWork.FlashCardSets.GetById(flashCardSetId);
        }


        public bool DeleteFlashCardSet(int flashCardSetId)
        {
 
             _unitOfWork.FlashCardSets.Delete(flashCardSetId);

            return true; 
        }

        public  IReadOnlyCollection<FlashCardSet> GetFlashCardSetsByLectureId(int? lectureId)
        {
            var allFlashCardSets = _unitOfWork.FlashCardSets.GetAll();
            if (lectureId != null)
            {
                var lectureSets = allFlashCardSets.Where(flashcardset => flashcardset.LectureId == lectureId);
                return lectureSets.ToList();
            }
            else
            {
                return allFlashCardSets;
            }
        }  

      

    }

}

