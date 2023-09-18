using System.Text.Json.Serialization;
using StudyZen.Common;

namespace StudyZen.FlashCards.Requests;

public sealed class CreateFlashCardRequest
{
 
    public int SetId { get; private set; }
    public string Question { get; }
    public string Answer { get; }

    public CreateFlashCardRequest(string? question, string? answer)
    {
        
        Question = question.ThrowIfRequestArgumentNull(nameof(question));
        Answer = answer.ThrowIfRequestArgumentNull(nameof(answer));
    }
}
