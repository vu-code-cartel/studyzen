using FluentValidation;
using StudyZen.Application.Validation;

public class FileMetadataValidator : AbstractValidator<FileMetadata>
{
    public FileMetadataValidator()
    {
        RuleFor(f => f.FileSize)
            .FileSize();
        RuleFor(f => f.FileName)
            .FileName();
        RuleFor(f => f.FileType)
            .FileType();
    }
}
