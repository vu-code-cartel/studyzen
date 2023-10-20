using System.Net;
using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using Hellang.Middleware.ProblemDetails;

namespace StudyZen.Api.Extensions;

public static class ProblemDetailsOptionsExtensions
{
    public static void MapFluentValidationException(
        this Hellang.Middleware.ProblemDetails.ProblemDetailsOptions options)
    {
        options.Map<ValidationException>((ctx, ex) =>
        {
            var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();

            var problemDetails = factory.CreateProblemDetails(
                ctx,
                (int)HttpStatusCode.UnprocessableEntity,
                detail: "One or more validation errors occurred.");

            problemDetails.Extensions["errors"] = FormatValidationErrors(ex.Errors);

            return problemDetails;
        });
    }

    /// <summary>
    /// Formats validation errors as JSON object. E.g.:
    /// id: ["NotNull"],
    /// stamp: {
    ///   timestamp: [
    ///     "NotNull"
    ///   ],
    ///   username: [
    ///     "NotNull", "MinLength"
    ///   ]
    /// },
    /// </summary>
    /// <param name="validationFailures">Validation errors.</param>
    /// <returns>Validation errors as JSON object.</returns>
    private static IDictionary<string, object> FormatValidationErrors(
        IEnumerable<ValidationFailure> validationFailures)
    {
        var errors = new Dictionary<string, object>();

        foreach (var failure in validationFailures)
        {
            var propertyNames = failure.PropertyName.Split('.').ToList();
            var errorList = CreateLayersByPropertyNames(propertyNames, errors);
            errorList.Add(failure.ErrorCode);
        }

        return errors;
    }

    private static IList<string> CreateLayersByPropertyNames(
        IList<string> propertyNames,
        IDictionary<string, object> currentLayer)
    {
        foreach (var name in propertyNames.SkipLast(1))
        {
            var propertyName = JsonNamingPolicy.CamelCase.ConvertName(name);

            var nextLayerExists = currentLayer.TryGetValue(propertyName, out var nextLayer);
            if (!nextLayerExists || nextLayer is null)
            {
                nextLayer = new Dictionary<string, object>();
                currentLayer.Add(propertyName, nextLayer);
            }

            currentLayer = (Dictionary<string, object>)nextLayer;
        }

        var lastPropertyName = JsonNamingPolicy.CamelCase.ConvertName(propertyNames.Last());
        var errorListExists = currentLayer.TryGetValue(lastPropertyName, out var errorList);
        if (!errorListExists || errorList is null)
        {
            errorList = new List<string>();
            currentLayer.Add(lastPropertyName, errorList);
        }

        return (List<string>)errorList;
    }
}