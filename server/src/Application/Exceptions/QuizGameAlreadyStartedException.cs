namespace StudyZen.Application.Exceptions;

public sealed class QuizGameAlreadyStartedException : IdentifiableException
{
    public QuizGameAlreadyStartedException() : base(ErrorCodes.QuizGameAlreadyStarted)
    {
    }
}
