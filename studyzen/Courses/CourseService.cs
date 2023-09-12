using StudyZen.Courses.Requests;
using StudyZen.Persistence;

namespace StudyZen.Courses;

public interface ICourseService
{
    int AddCourse(CreateCourseRequest request);
}

public sealed class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;

    public CourseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public int AddCourse(CreateCourseRequest request)
    {
        _unitOfWork.Courses.Update(new Course("", ""));

        throw new NotImplementedException();
    }
}