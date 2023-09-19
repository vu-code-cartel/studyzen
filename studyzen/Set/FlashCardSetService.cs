using StudyZen.FlashCards.Requests;
using StudyZen.Persistence;
using StudyZen.FlashCardSetClass;


namespace StudyZen.FlashCardSets
{

    public interface IFlashCardSetService
    {
        int AddFlashCardSet(CreateFlashCardSetRequest request); 
        FlashCardSet GetFlashCardSet(int flashCardSetId);
        public bool DeleteFlashCardSet(int flashCardSetId);

        public List<FlashCardSet> GetAllFlashCardSets();
        public void UpdateFlashCardSet(FlashCardSet flashCardSet);

    }

    public sealed class FlashCardSetService : IFlashCardSetService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FlashCardSetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<FlashCardSet> GetAllFlashCardSets()
        {

            return _unitOfWork.FlashCardSets.GetAll();
            
        }

        public int AddFlashCardSet(CreateFlashCardSetRequest request)
    {
    
        FlashCardSet flashCardSet = new FlashCardSet(request.SetName, request.Color, request.LectureId);

    
        _unitOfWork.FlashCardSets.Add(flashCardSet);

        return flashCardSet.Id;
    }



        public FlashCardSet GetFlashCardSet(int flashCardSetId)
        {
             return _unitOfWork.FlashCardSets.GetById(flashCardSetId);
        }


        public bool DeleteFlashCardSet(int flashCardSetId)
        {
            var flashCardSet = GetFlashCardSet(flashCardSetId);
            if (flashCardSet == null)
            {
                return false;
            }
 
             _unitOfWork.FlashCardSets.Delete(flashCardSetId);

            return true; 
        }

         public void UpdateFlashCardSet(FlashCardSet flashCardSet)
        {
            _unitOfWork.FlashCardSets.Update(flashCardSet);
        }       

      

    }

}

