using FluentValidation;
using System.Text.RegularExpressions;

namespace StudyZen.Application.Validation;

public static class FileMetadataValidationExtensions
{
    public static IRuleBuilderOptions<T, long> FileSize<T>(this IRuleBuilder<T, long> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage("File size must be greater than zero")
            .LessThanOrEqualTo(1024 * 1024 * 5)
            .WithMessage("File size must be less than or equal to 5 MB");
    }

    public static IRuleBuilderOptions<T, string?> FileName<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .NotEmpty()
            .Must(IsCsvFile).WithMessage("Only CSV files are allowed");
    }

    static bool IsCsvFile(string? fileName)
    {
        return string.IsNullOrWhiteSpace(fileName) ? false : Regex.IsMatch(fileName, @"\.csv$", RegexOptions.IgnoreCase);
    }
}