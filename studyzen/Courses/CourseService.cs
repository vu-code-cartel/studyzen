using StudyZen.Courses.Requests;
using StudyZen.Persistence;

namespace StudyZen.Courses;

public interface ICourseService
{
    Course AddCourse(CreateCourseRequest request);
    Course? GetCourseById(int id);
    Course? UpdateCourse(UpdateCourseRequest request, int id);
    void DeleteCourse(int id);
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

    public Course? GetCourseById(int id)
    {
        var course = _unitOfWork.Courses.GetById(id);
        return course;
    }

    public Course? UpdateCourse(UpdateCourseRequest request,int id)
    {
        var oldCourse = _unitOfWork.Courses.GetById(id);
        if (oldCourse != null)
        {
            oldCourse.Name = request.Name ?? oldCourse.Name;
            oldCourse.Description = request.Description ?? oldCourse.Description;
            _unitOfWork.Courses.Update(oldCourse);
            return oldCourse;
        }
        return null;
    }

    public void DeleteCourse(int id)
    {
            _unitOfWork.Courses.Delete(id);
    }
}