﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using StudyZen.Application.Repositories;

namespace StudyZen.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private bool _disposed;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public ICourseRepository Courses { get; }
    public ILectureRepository Lectures { get; }
    public IFlashcardSetRepository FlashcardSets { get; }
    public IFlashcardRepository Flashcards { get; }
    public IQuizRepository Quizzes { get; }
    public IQuizQuestionRepository QuizQuestions { get; }
    public IQuizAnswerRepository QuizAnswers { get; }
    public IRefreshTokenRepository RefreshTokens { get; }

    // TODO: change these to save changes interceptors after lab 2
    // https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors   

    public UnitOfWork(
        ApplicationDbContext dbContext,
        IHttpContextAccessor httpContextAccessor,
        ICourseRepository courses,
        ILectureRepository lectures,
        IFlashcardSetRepository flashcardSets,
        IFlashcardRepository flashcards,
        IQuizRepository quizzes,
        IQuizQuestionRepository quizQuestions,
        IQuizAnswerRepository quizAnswers,
        IRefreshTokenRepository refreshTokens)
    {
        _dbContext = dbContext;

        _httpContextAccessor = httpContextAccessor;

        Courses = courses;
        Lectures = lectures;
        FlashcardSets = flashcardSets;
        Flashcards = flashcards;
        Quizzes = quizzes;
        QuizQuestions = quizQuestions;
        QuizAnswers = quizAnswers;
        RefreshTokens = refreshTokens;
    }

    public async Task<int> SaveChanges()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public IDbContextTransaction BeginTransaction() => _dbContext.Database.BeginTransaction();

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _dbContext.Dispose();
        _disposed = true;
    }
}