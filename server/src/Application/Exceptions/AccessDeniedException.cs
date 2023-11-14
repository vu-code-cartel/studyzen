namespace StudyZen.Application.Exceptions;

public sealed class AccessDeniedException : Exception
{
    public AccessDeniedException()
        : base("User has no rights to modify the content")
    {
    }
}