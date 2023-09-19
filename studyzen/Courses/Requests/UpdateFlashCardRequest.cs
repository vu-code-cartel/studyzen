namespace StudyZen.FlashCards.Requests
{
    public class UpdateFlashCardRequest
    {
        public int FlashCardSetId {get; set;}
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}