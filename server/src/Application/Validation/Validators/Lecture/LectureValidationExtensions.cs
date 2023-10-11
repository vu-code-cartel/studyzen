using FluentValidation;
using StudyZen.Application.Services;
using StudyZen.Domain.Constraints;

namespace StudyZen.Application.Validation;

public static class LectureValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> LectureName<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.MustNotBeEmpty)
            .MaximumLength(LectureConstraints.NameMaxLength)
            .WithErrorCode(ValidationErrorCodes.TooLong);
    }

    public static IRuleBuilderOptions<T, string?> LectureContent<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .WithErrorCode(ValidationErrorCodes.MustNotBeNull)
            .MaximumLength(LectureConstraints.ContentMaxLength)
            .WithErrorCode(ValidationErrorCodes.TooLong);
    }

    public static IRuleBuilderOptions<T, int?> OptionalLectureId<T>(this IRuleBuilder<T, int?> ruleBuilder, ILectureService lectureService)
    {
        return ruleBuilder
            .Must(id => !id.HasValue || lectureService.GetLectureById(id.Value) is not null)
            .WithErrorCode(ValidationErrorCodes.NotFound);
    }
}