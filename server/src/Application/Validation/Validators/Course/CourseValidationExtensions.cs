using FluentValidation;
using StudyZen.Application.Services;
using StudyZen.Domain.Constraints;

namespace StudyZen.Application.Validation;

public static class CourseValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> CourseName<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotNullOrWhitespaceRule()
            .MaxLengthRule(CourseConstraints.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> CourseDescription<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotNullRule()
            .MaxLengthRule(CourseConstraints.DescriptionMaxLength);
    }

    public static IRuleBuilderOptions<T, int> CourseId<T>(this IRuleBuilder<T, int> ruleBuilder, ICourseService courseService)
    {
        return ruleBuilder
            .Must(id => courseService.GetCourseById(id) is not null)
            .WithErrorCode(ValidationErrorCodes.NotFound);
    }
}