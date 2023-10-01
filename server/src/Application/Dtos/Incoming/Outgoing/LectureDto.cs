namespace StudyZen.Application.Dtos;

public class LectureDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
}