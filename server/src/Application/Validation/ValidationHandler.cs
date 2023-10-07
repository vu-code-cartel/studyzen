using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Validation;

public class ValidationHandler
{
    private readonly IServiceProvider _serviceProvider;
    public ValidationHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public void Validate<T>(T instance)
    {
        var validator = _serviceProvider.GetService<IValidator<T>>();
        if (validator is null)
        {
            throw new InvalidOperationException();
        }
        validator.ValidateAndThrow(instance);
    }
}