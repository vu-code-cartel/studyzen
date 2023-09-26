namespace StudyZen.Application.Dtos;

public sealed record CreateLectureDto(int CourseId, string Name, string Content);