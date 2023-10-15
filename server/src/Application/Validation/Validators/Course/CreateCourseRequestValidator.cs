using FluentValidation;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Validation;

public class CreateCourseRequestValidator : AbstractValidator<CreateCourseDto>
{
    public CreateCourseRequestValidator()
    {
        RuleFor(c => c.Name)
            .CourseName();
        RuleFor(c => c.Description)
            .CourseDescription();
    }
}