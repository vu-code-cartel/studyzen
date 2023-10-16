using FluentValidation;
using StudyZen.Application.Services;
using StudyZen.Domain.Constraints;

namespace StudyZen.Application.Validation;

public static class LectureValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> LectureName<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MustNotBeNullOrWhitespace()
            .LengthMustNotExceed(LectureConstraints.NameMaxLength);
    }

    public static IRuleBuilderOptions<T, string?> LectureContent<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MustNotBeNull()
            .LengthMustNotExceed(LectureConstraints.ContentMaxLength);
    }

    public static IRuleBuilderOptions<T, int?> OptionalLectureId<T>(this IRuleBuilder<T, int?> ruleBuilder, ILectureService lectureService)
    {
        return ruleBuilder
            .MustAsync(async (id, cancellationToken) =>
                !id.HasValue || (await lectureService.GetLectureById(id.Value)) is not null)
            .WithErrorCode(ValidationErrorCodes.NotFound);
    }

}