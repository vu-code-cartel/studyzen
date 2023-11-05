namespace StudyZen.Application.Exceptions;

public sealed class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string identificationType, string identificationValue)
        : base($"User with {identificationType} {identificationValue} already exists")
    {
    }
}