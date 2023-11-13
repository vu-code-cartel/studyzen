namespace StudyZen.Application.Exceptions;

public sealed class UsernameTakenException : IdentifiableException
{
    public UsernameTakenException() : base(ErrorCodes.UsernameTaken)
    {
    }
}
