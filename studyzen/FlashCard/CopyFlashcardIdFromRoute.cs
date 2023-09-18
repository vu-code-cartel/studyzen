using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StudyZen.FlashCards;

public class CopyFlashcardIdFromRoute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.ContainsKey("flashcardId") && context.ActionArguments["flashcardId"] is int flashcardId)
        {
            if (context.ModelState.TryGetValue("request", out var entry) && entry != null && entry.RawValue is FlashCard request)
            {
               request.Id = flashcardId;
            }
        }

        base.OnActionExecuting(context);
    }
}
