using Studyzen.Lectures;
using StudyZen.Lectures.Requests;
using StudyZen.Persistence;
using StudyZen.Utils;

namespace StudyZen.Lectures;

public interface ILectureService
{
    Lecture AddLecture(CreateLectureRequest request);
    Lecture? GetLectureById(int lectureId);
    IReadOnlyCollection<Lecture> GetAllLectures();
    IReadOnlyCollection<Lecture> GetLecturesByCourseId(int courseId);
    Lecture? UpdateLectureById(int lectureId, UpdateLectureRequest request);
    void DeleteLectureById(int lectureId);
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

    public IReadOnlyCollection<Lecture> GetAllLectures()
    {
        return _unitOfWork.Lectures.GetAll();
    }

    public IReadOnlyCollection<Lecture> GetLecturesByCourseId(int courseId)
    {
        var allLectures = _unitOfWork.Lectures.GetAll();
        var courseLectures = allLectures.Where(lecture => lecture.CourseId == courseId);
        return courseLectures.ToList();
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
        DeleteFlashcardSetsFromLecture(lectureId);
        _unitOfWork.Lectures.Delete(lectureId);
    }

    private void DeleteFlashcardSetsFromLecture(int lectureId)
    {
        var allFlashcardSets = _unitOfWork.FlashcardSets.GetAll();
        var lectureFlashcardSets = allFlashcardSets.Where(flashcardSet => lectureId == flashcardSet.LectureId);
        foreach (var lectureFlashcardSet in lectureFlashcardSets)
        {
            lectureFlashcardSet.LectureId = null;
            _unitOfWork.FlashcardSets.Update(lectureFlashcardSet);
        }
    }
}