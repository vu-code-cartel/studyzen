using StudyZen.Courses.Requests;
using StudyZen.Persistence;

namespace StudyZen.Courses;

public interface ICourseService
{
    Course CreateCourse(CreateCourseRequest request);
    Course? GetCourseById(int id);
    IReadOnlyCollection<Course> GetAllCourses();
    Course? UpdateCourse(int id, UpdateCourseRequest request);
    void DeleteCourse(int id);
}

public sealed class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;

    public CourseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Course CreateCourse(CreateCourseRequest request)
    {
        var newCourse = new Course(request.Name, request.Description);
        _unitOfWork.Courses.Add(newCourse);
        return newCourse;
    }

    public Course? GetCourseById(int id)
    {
        var course = _unitOfWork.Courses.GetById(id);
        return course;
    }

    public Course? UpdateCourse(int id, UpdateCourseRequest request)
    {
        var course = _unitOfWork.Courses.GetById(id);
        if (course is null)
        {
            return null;
        }

        course.Name = request.Name ?? course.Name;
        course.Description = request.Description ?? course.Description;
        _unitOfWork.Courses.Update(course);

        return course;
    }

    public void DeleteCourse(int id)
    {
        DeleteLecturesByCourseId(id);
        _unitOfWork.Courses.Delete(id);
    }

    public IReadOnlyCollection<Course> GetAllCourses()
    {
        var allCourses = _unitOfWork.Courses.GetAll();
        return allCourses;
    }

    private void DeleteLecturesByCourseId(int? courseId)
    {
        var allLectures = _unitOfWork.Lectures.GetAll();
        var courseLectures = allLectures.Where(lecture => lecture.CourseId == courseId);

        foreach (var courseLecture in courseLectures)
        {
            _unitOfWork.Lectures.Delete(courseLecture.Id);
        }
    }
}