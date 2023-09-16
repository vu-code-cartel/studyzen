using Studyzen.Lectures;
using StudyZen.Lectures.Requests;
using StudyZen.Persistence;

namespace StudyZen.Lectures;

public interface ILectureService
{
    Lecture AddLecture(int courseId, CreateLectureRequest request);
    Lecture? GetLectureById(int lectureId);
    IEnumerable<Lecture> GetLecturesByCourseId(int? courseId);
}

public sealed class LectureService : ILectureService
{
    private readonly IUnitOfWork _unitOfWork;

    public LectureService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Lecture AddLecture(int courseId, CreateLectureRequest request)
    {
        Lecture newLecture = new Lecture(courseId, request.Name, request.Content);
        _unitOfWork.Lectures.Add(newLecture);
        return newLecture;
    }
    public Lecture? GetLectureById(int lectureId)
    {
        Lecture? requestedLecture = _unitOfWork.Lectures.GetById(lectureId);
        return requestedLecture;
    }
    public IEnumerable<Lecture> GetLecturesByCourseId(int? courseId)
    {
        IEnumerable<Lecture> allLectures = _unitOfWork.Lectures.GetAll();
        if (courseId != null)
        {
            IEnumerable<Lecture> courseLectures = allLectures.Where(lecture => lecture.CourseId == courseId);
            return courseLectures;
        }
        else
        {
            return allLectures;
        }
    }
}