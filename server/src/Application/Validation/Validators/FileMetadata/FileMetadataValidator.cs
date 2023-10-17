using FluentValidation;
using StudyZen.Application.Dtos;
using StudyZen.Application.Validation;

public class FileMetadataValidator : AbstractValidator<FileMetadata>
{
    public FileMetadataValidator()
    {
        RuleFor(f => f.Size)
            .FileSize();
        RuleFor(f => f.Name)
            .FileName();
    }
}