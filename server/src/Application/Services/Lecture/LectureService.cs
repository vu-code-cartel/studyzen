using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using FluentValidation;
using StudyZen.Application.Validation;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public sealed class LectureService : ILectureService
{
    private readonly ILectureRepository _lectures;
    private readonly IFlashcardSetRepository _flashcardSets;
    private readonly ValidationHandler _validationHandler;

    public LectureService(ILectureRepository lectures, IFlashcardSetRepository flashcardSets, ValidationHandler validationHandler)
    {
        _lectures = lectures;
        _flashcardSets = flashcardSets;
        _validationHandler = validationHandler;
    }

    public Lecture CreateLecture(CreateLectureDto dto)
    {
        _validationHandler.Validate(dto);
        var newLecture = new Lecture(dto.CourseId, dto.Name, dto.Content);
        _lectures.Add(newLecture);
        return newLecture;
    }

    public Lecture? GetLectureById(int lectureId)
    {
        var lecture = _lectures.GetById(lectureId);
        return lecture;
    }

    public IReadOnlyCollection<Lecture> GetLecturesByCourseId(int courseId)
    {
        var allLectures = _lectures.GetAll();
        var courseLectures = allLectures.Where(l => l.CourseId == courseId).ToList();
        return courseLectures;
    }

    public bool UpdateLecture(int lectureId, UpdateLectureDto dto)
    {
        var lecture = _lectures.GetById(lectureId);
        if (lecture is null)
        {
            return false;
        }

        _validationHandler.Validate(dto);
        lecture.Name = dto.Name ?? lecture.Name;
        lecture.Content = dto.Content ?? lecture.Content;
        _lectures.Update(lecture);

        return true;
    }

    public bool DeleteLecture(int lectureId)
    {
        DeleteFlashcardSetsFromLecture(lectureId);
        return _lectures.Delete(lectureId);
    }

    private void DeleteFlashcardSetsFromLecture(int lectureId)
    {
        var allFlashcardSets = _flashcardSets.GetAll();
        var lectureFlashcardSets = allFlashcardSets.Where(fs => fs.LectureId == lectureId);

        foreach (var lectureFlashcardSet in lectureFlashcardSets)
        {
            lectureFlashcardSet.LectureId = null;
            _flashcardSets.Update(lectureFlashcardSet);
        }
    }
}