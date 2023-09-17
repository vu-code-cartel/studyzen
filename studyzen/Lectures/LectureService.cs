using Studyzen.Lectures;
using StudyZen.Lectures.Requests;
using StudyZen.Persistence;
using StudyZen.Utils;

namespace StudyZen.Lectures;

public interface ILectureService
{
    Lecture AddLecture(CreateLectureRequest request);
    Lecture? GetLectureById(int lectureId);
    IReadOnlyCollection<Lecture> GetLecturesByCourseId(int? courseId);
    Lecture? UpdateLectureById(int lectureId, UpdateLectureRequest request);
    public void DeleteLectureById(int lectureId);
}

public sealed class LectureService : ILectureService
{
    private readonly IUnitOfWork _unitOfWork;

    public LectureService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Lecture AddLecture(CreateLectureRequest request)
    {
        var newLecture = new Lecture(request.CourseId, request.Name, request.Content);
        _unitOfWork.Lectures.Add(newLecture);
        return newLecture;
    }

    public Lecture? GetLectureById(int lectureId)
    {
        var requestedLecture = _unitOfWork.Lectures.GetById(lectureId);
        return requestedLecture;
    }

    public IReadOnlyCollection<Lecture> GetLecturesByCourseId(int? courseId)
    {
        var allLectures = _unitOfWork.Lectures.GetAll();
        if (courseId != null)
        {
            var courseLectures = allLectures.Where(lecture => lecture.CourseId == courseId);
            return courseLectures.ToList();
        }
        else
        {
            return allLectures;
        }
    }

    public Lecture? UpdateLectureById(int lectureId, UpdateLectureRequest request)
    {
        var toBeUpdatedLecture = _unitOfWork.Lectures.GetById(lectureId);
        if (toBeUpdatedLecture == null)
        {
            return null;
        }
        if (request.Name != null)
        {
            toBeUpdatedLecture.Name = request.Name;
        }
        if (request.Content != null)
        {
            toBeUpdatedLecture.Content = request.Content;
        }
        _unitOfWork.Lectures.Update(toBeUpdatedLecture);
        return toBeUpdatedLecture;
    }

    public void DeleteLectureById(int lectureId)
    {
        _unitOfWork.Lectures.Delete(lectureId);
    }
}