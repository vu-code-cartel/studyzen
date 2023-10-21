using FluentValidation;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Validation;

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