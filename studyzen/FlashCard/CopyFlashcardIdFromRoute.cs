using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StudyZen.FlashCards;

public class CopyFlashCardIdFromRoute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.ContainsKey("flashCardId") && context.ActionArguments["flashCardId"] is int flashCardId)
        {
            if (context.ModelState.TryGetValue("request", out var entry) && entry != null && entry.RawValue is FlashCard request)
            {
               request.Id = flashCardId;
            }
        }

        base.OnActionExecuting(context);
    }
}
