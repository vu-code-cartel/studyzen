using StudyZen.Courses.Requests;
using StudyZen.Persistence;

namespace StudyZen.Courses;

public interface ICourseService
{
    Course AddCourse(CreateCourseRequest request);
    Course GetCourseById(int id);
}

public sealed class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;

    public CourseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Course AddCourse(CreateCourseRequest request)
    {
        var newCourse = new Course(request.Name,request.Description);
        _unitOfWork.Courses.Add(newCourse);
        return newCourse;
    }

    public Course GetCourseById(int id)
    {
        var course = _unitOfWork.Courses.GetById(id);
        return course;
    }
}