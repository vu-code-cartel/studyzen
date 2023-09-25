using StudyZen.Lectures.Requests;
using StudyZen.Persistence;

namespace StudyZen.Lectures;

public interface ILectureService
{
    Lecture CreateLecture(CreateLectureRequest request);
    Lecture? GetLectureById(int lectureId);
    IReadOnlyCollection<Lecture> GetLecturesByCourseId(int courseId);
    Lecture? UpdateLecture(int lectureId, UpdateLectureRequest request);
    void DeleteLecture(int lectureId);
}

public sealed class LectureService : ILectureService
{
    private readonly IUnitOfWork _unitOfWork;

    public LectureService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Lecture CreateLecture(CreateLectureRequest request)
    {
        var newLecture = new Lecture(request.CourseId, request.Name, request.Content);
        _unitOfWork.Lectures.Add(newLecture);
        return newLecture;
    }

    public Lecture? GetLectureById(int lectureId)
    {
        var lecture = _unitOfWork.Lectures.GetById(lectureId);
        return lecture;
    }

    public IReadOnlyCollection<Lecture> GetLecturesByCourseId(int courseId)
    {
        var allLectures = _unitOfWork.Lectures.GetAll();
        var courseLectures = allLectures.Where(lecture => lecture.CourseId == courseId).ToList();
        return courseLectures;
    }

    public Lecture? UpdateLecture(int lectureId, UpdateLectureRequest request)
    {
        var lecture = _unitOfWork.Lectures.GetById(lectureId);
        if (lecture is null)
        {
            return null;
        }

        lecture.Name = request.Name ?? lecture.Name;
        lecture.Content = request.Content ?? lecture.Content;
        _unitOfWork.Lectures.Update(lecture);

        return lecture;
    }

    public void DeleteLecture(int lectureId)
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