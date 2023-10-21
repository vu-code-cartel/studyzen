using FluentValidation;
using FluentValidation.Results;
using StudyZen.Application.Validation;

namespace StudyZen.Application.Exceptions;

public class IncorrectArgumentCountException : ValidationException
{
    public IncorrectArgumentCountException(params string[] propertyNames)
        : base(new[]
        {
            // Join nested property names by dot - FluentValidation convention
            new ValidationFailure(string.Join('.', propertyNames), "Argument count is incorrect.")
            {
                ErrorCode = ValidationErrorCodes.IncorrectArgumentCount
            }
        })
    {
    }
}