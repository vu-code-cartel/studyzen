using Studyzen.Lectures;
using StudyZen.Lectures.Requests;
using StudyZen.Persistence;
using StudyZen.Utils;

namespace StudyZen.Lectures;

public interface ILectureService
{
    Lecture AddLecture(CreateLectureRequest request);
    Lecture? GetLectureById(int lectureId);
    IEnumerable<Lecture> GetLecturesByCourseId(int? courseId);
    Lecture? UpdateLectureById(int lectureId, string? name, string? content);
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
        Lecture newLecture = new Lecture(request.CourseId, request.Name, request.Content);
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

    public Lecture? UpdateLectureById(int lectureId, string? name, string? content)
    {
        Lecture? toBeUpdatedLecture = _unitOfWork.Lectures.GetById(lectureId);
        if (toBeUpdatedLecture != null)
        {
            if (name != null)
            {
                toBeUpdatedLecture.Name = name;
            }
            if (content != null)
            {
                toBeUpdatedLecture.Content = content;
            }
            toBeUpdatedLecture.UpdatedBy = new UserActionStamp();
            _unitOfWork.Lectures.Update(toBeUpdatedLecture);
        }
        return toBeUpdatedLecture;
    }

    public void DeleteLectureById(int lectureId)
    {
        _unitOfWork.Lectures.Delete(lectureId);
    }
}