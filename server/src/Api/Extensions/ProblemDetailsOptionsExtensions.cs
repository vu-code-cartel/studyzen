using FluentValidation;
using FluentValidation.Results;
using Hellang.Middleware.ProblemDetails;
using System.Net;
using System.Text.Json;
using StudyZen.Application.Validation;

namespace StudyZen.Api.Extensions;

public static class ProblemDetailsOptionsExtensions
{
    public static void MapValidationException(
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
    /// "stamp": {
    ///   "timestamp": [
    ///     {
    ///       "errorCode: "NotNull",
    ///       "detail": "Must not be null."
    ///     }
    ///   ],
    ///   "username": [
    ///     {
    ///       "errorCode: "MinLength",
    ///       "detail": "The username is too short."
    ///     }
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
            var propertyNames = failure.PropertyName
                .Split('.')
                .Select(JsonNamingPolicy.CamelCase.ConvertName)
                .ToList();

            var errorMetadata = new ValidationErrorMetadata(failure.ErrorCode, failure.ErrorMessage);
            var errorList = GetOrCreateErrorList(propertyNames, errors);
            errorList.Add(errorMetadata);
        }

        return errors;
    }

    private static IList<ValidationErrorMetadata> GetOrCreateErrorList(
        IList<string> propertyNames,
        IDictionary<string, object> currentLayer)
    {
        foreach (var propertyName in propertyNames.SkipLast(1))
        {
            var nextLayerExists = currentLayer.TryGetValue(propertyName, out var nextLayer);
            if (!nextLayerExists || nextLayer is null)
            {
                nextLayer = new Dictionary<string, object>();
                currentLayer.Add(propertyName, nextLayer);
            }

            currentLayer = (Dictionary<string, object>)nextLayer;
        }

        var lastPropertyName = propertyNames.Last();
        var lastLayerExists = currentLayer.TryGetValue(lastPropertyName, out var errorList);
        if (!lastLayerExists || errorList is null)
        {
            errorList = new List<ValidationErrorMetadata>();
            currentLayer.Add(lastPropertyName, errorList);
        }

        return (List<ValidationErrorMetadata>)errorList;
    }
}