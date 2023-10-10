﻿using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Services;

namespace StudyZen.Infrastructure.Persistence;

public sealed class FlashcardRepository : Repository<Flashcard>, IFlashcardRepository
{
    public FlashcardRepository(IFileService fileService, ApplicationDbContext dbContext) : base("flashcards", fileService, dbContext)
    {
    }
}