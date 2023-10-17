using FluentValidation;
using StudyZen.Application.Validation;
using StudyZen.Application.ValueObjects;

public class FileMetadataValidator : AbstractValidator<FileMetadata>
{
    public FileMetadataValidator()
    {
        RuleFor(f => f.FileSize)
            .FileSize();
        RuleFor(f => f.FileName)
            .FileName();
    }
}
