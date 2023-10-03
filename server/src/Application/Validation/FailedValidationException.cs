public sealed class FailedValidationException : ArgumentException
{
    public FailedValidationException(string? message = null) : base(message)
    {
    }
}