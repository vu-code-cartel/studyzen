using Studyzen.Courses.Requests;

namespace Studyzen.Courses;

public interface ICourseService
{
    Task<int> AddCourseAsync(CreateCourseRequest request);
}

public sealed class CourseService : ICourseService
{
    public async Task<int> AddCourseAsync(CreateCourseRequest request)
    {
        var course = new Course(default, request.Name, request.Description);

        return course.Id;
    }
}