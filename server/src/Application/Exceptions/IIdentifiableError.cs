namespace StudyZen.Application.Exceptions;

public abstract class IdentifiableException : Exception
{
    public string ErrorCode { get; }

    protected IdentifiableException(string errorCode)
    {
        ErrorCode = errorCode;
    }
}
