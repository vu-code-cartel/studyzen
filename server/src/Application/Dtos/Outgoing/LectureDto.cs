using StudyZen.Domain.Entities;
using StudyZen.Domain.ValueObjects;

namespace StudyZen.Application.Dtos;

public sealed record LectureDto(
    int Id,
    int CourseId,
    string Name,
    string Content,
    UserActionStamp CreatedBy,
    UserActionStamp UpdatedBy);