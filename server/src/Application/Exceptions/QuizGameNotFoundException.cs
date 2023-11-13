namespace StudyZen.Application.Exceptions;

public sealed class QuizGameNotFoundException : IdentifiableException
{
    public QuizGameNotFoundException() : base(ErrorCodes.QuizGameNotFound)
    {
    }
}
