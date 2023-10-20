using System.Net;
using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using Hellang.Middleware.ProblemDetails;

namespace StudyZen.Api.Extensions;

public static class ProblemDetailsOptionsExtensions
{
    public static void MapFluentValidationException(this Hellang.Middleware.ProblemDetails.ProblemDetailsOptions options)
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
    private static IDictionary<string, object> FormatValidationErrors(IEnumerable<ValidationFailure> validationFailures)
    {
        var errors = new Dictionary<string, object>();

        foreach (var failure in validationFailures)
        {
            var currentLayer = errors;
            var propertyNames = failure.PropertyName.Split('.');

            if (propertyNames.Length > 1)
            {
                for (var i = 0; i < propertyNames.Length - 1; i++)
                {
                    var propertyName = JsonNamingPolicy.CamelCase.ConvertName(propertyNames[i]);

                    var nextLayerExists = currentLayer.TryGetValue(propertyName, out var nextLayer);
                    if (!nextLayerExists || nextLayer is null)
                    {
                        nextLayer = new Dictionary<string, object>();
                        currentLayer.Add(propertyName, nextLayer);
                    }

                    currentLayer = (Dictionary<string, object>)nextLayer;
                }
            }

            var lastPropertyName = JsonNamingPolicy.CamelCase.ConvertName(propertyNames.Last());
            var errorListExists = currentLayer.TryGetValue(lastPropertyName, out var errorList);
            if (errorListExists && errorList is not null)
            {
                ((List<string>)errorList).Add(failure.ErrorCode);
            }
            else
            {
                currentLayer.Add(lastPropertyName, new List<string> { failure.ErrorCode });
            }
        }

        return errors;
    }
}