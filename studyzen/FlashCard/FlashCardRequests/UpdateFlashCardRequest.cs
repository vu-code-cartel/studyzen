using StudyZen.Common;

namespace StudyZen.FlashCards.Requests;
public sealed class UpdateFlashCardRequest
{
 
    public string? Question { get; }
    public string? Answer { get; }


    public UpdateFlashCardRequest(int flashCardSetId, string? question, string? answer)
    {
        Question = question;
        Answer = answer;
    }
}