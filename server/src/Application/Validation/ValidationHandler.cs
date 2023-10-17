using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace StudyZen.Application.Validation;

public class ValidationHandler
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task ValidateAsync<T>(T instance)
    {
        var validator = _serviceProvider.GetService<IValidator<T>>();
        if (validator is null)
        {
            throw new InvalidOperationException($"Validator for '{typeof(T).Name}' is not registered.");
        }

        await validator.ValidateAndThrowAsync(instance);
    }
}