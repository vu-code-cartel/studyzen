using FluentValidation;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Validation;

public class UpdateCourseRequestValidator : AbstractValidator<UpdateCourseDto>
{
    public UpdateCourseRequestValidator()
    {
        RuleFor(c => c.Name)
            .CourseName()
            .Unless(c => c.Name is null);
        RuleFor(course => course.Description)
            .CourseDescription()
            .Unless(c => c.Description is null);
    }
}