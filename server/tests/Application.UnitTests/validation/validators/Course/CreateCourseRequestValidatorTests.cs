using FluentValidation.TestHelper;
using StudyZen.Domain.Constraints;
using StudyZen.Application.Dtos;
using StudyZen.Application.Validation;

[TestFixture]
public class CreateCourseRequestValidatorTests
{
    private CreateCourseRequestValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new CreateCourseRequestValidator();
    }

    [Test]
    public void ShouldNotHaveValidationError_WhenNameAndDescriptionAreSpecifiedCorrectly()
    {
        var courseDto = new CreateCourseDto("Valid Course Name", "Valid Course Description");

        var result = _validator.TestValidate(courseDto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void ShouldHaveValidationError_WhenNameIsEmpty()
    {
        var courseDto = new CreateCourseDto("", "Valid Course Description");
        var result = _validator.TestValidate(courseDto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void ShouldHaveValidationError_WhenNameIsNull()
    {
        var courseDto = new CreateCourseDto(null, "Valid Course Description");
        var result = _validator.TestValidate(courseDto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void ShouldHaveValidationError_WhenNameIsTooLong()
    {
        var courseDto = new CreateCourseDto(new string('a', CourseConstraints.NameMaxLength + 1), "Valid Course Description");
        var result = _validator.TestValidate(courseDto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void ShouldHaveValidationError_WhenDescriptionIsTooLong()
    {
        var courseDto = new CreateCourseDto("Valid Course Name", new string('a', CourseConstraints.DescriptionMaxLength + 1));
        var result = _validator.TestValidate(courseDto);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public void ShouldHaveValidationError_WhenDescriptionIsNull()
    {
        var courseDto = new CreateCourseDto("Valid Course Name", null);
        var result = _validator.TestValidate(courseDto);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }
}