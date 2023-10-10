namespace StudyZen.Api.Exceptions;

public sealed class ImportFailedException : Exception
{
    public ImportFailedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

