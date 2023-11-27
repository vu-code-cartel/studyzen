using System.Diagnostics.CodeAnalysis;

namespace StudyZen.Application.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string identificationType, string identificationValue)
        : base($"User with {identificationType} {identificationValue} already exists")
    {
    }
}