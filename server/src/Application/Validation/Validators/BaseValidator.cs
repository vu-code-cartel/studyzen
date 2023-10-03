using FluentValidation;
using FluentValidation.Results;

namespace StudyZen.Application.Validation.Validators;
public class BaseValidator<T> : AbstractValidator<T>
{
    protected override void RaiseValidationException(ValidationContext<T> context, ValidationResult result)
    {
        var ex = new ValidationException(result.Errors);
        throw new FailedValidationException(result.Errors.FirstOrDefault()?.ErrorMessage);
    }
    protected bool NullEmptyOrNotWhitespace(string? value)
    {
        return value is null || value == "" || !string.IsNullOrWhiteSpace(value);
    }
    protected bool NullOrNotWhiteSpace(string? value)
    {
        return value is null || !string.IsNullOrWhiteSpace(value);
    }
}