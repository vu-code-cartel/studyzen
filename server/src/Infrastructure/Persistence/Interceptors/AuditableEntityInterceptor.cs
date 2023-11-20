using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using StudyZen.Application.Services;
using StudyZen.Domain.Interfaces;
using StudyZen.Domain.ValueObjects;

namespace StudyZen.Infrastructure.Persistence;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public AuditableEntityInterceptor(ICurrentUserAccessor currentUserAccessor)
    {
        _currentUserAccessor = currentUserAccessor;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    private void UpdateAuditFields(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var entries = context.ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                SetCreateStamp(entry.Entity);
            }
            else if (entry.State == EntityState.Modified)
            {
                SetUpdateStamp(entry.Entity);
            }

        }
    }

    public void SetCreateStamp(object instance)
    {
        if (instance is IAuditable auditable)
        {
            auditable.CreatedBy = new UserActionStamp(_currentUserAccessor.GetUserId(), DateTime.UtcNow);
            auditable.UpdatedBy = auditable.CreatedBy with { }; // shallow copy
        }
    }

    public void SetUpdateStamp(object instance)
    {
        if (instance is IAuditable auditable)
        {
            auditable.UpdatedBy = new UserActionStamp(_currentUserAccessor.GetUserId(), DateTime.UtcNow);
        }
    }
}
