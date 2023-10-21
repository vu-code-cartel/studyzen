using FluentValidation;
using System.Text.RegularExpressions;

namespace StudyZen.Application.Validation;

public static class FileMetadataValidationExtensions
{
    public static IRuleBuilderOptions<T, long> FileSize<T>(this IRuleBuilder<T, long> ruleBuilder)
    {
        return ruleBuilder
            .MustBeGreaterThan(0)
            .MustBeLessThan(1024 * 1024 * 5);
    }

    public static IRuleBuilderOptions<T, string?> FileName<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MustNotBeNullOrWhitespace()
            .NotNull()
            .NotEmpty()
            .Must(IsCsvFile)
            .WithErrorCode(ValidationErrorCodes.InvalidFileFormat);
    }

    private static bool IsCsvFile(string? fileName)
    {
        return !string.IsNullOrWhiteSpace(fileName) && Regex.IsMatch(fileName, @"\.csv$", RegexOptions.IgnoreCase);
    }
}