using StudyZen.Application.Services;
using StudyZen.Domain.Interfaces;
using StudyZen.Domain.ValueObjects;

namespace StudyZen.Infrastructure.Persistence;

public static class AuditableEntityInterceptor
{
    public static void SetCreateStamp(object instance, IUserContextService userContextService)
    {
        if (instance is IAuditable auditable)
        {
            auditable.CreatedBy = new UserActionStamp(userContextService.ApplicationUserId!, DateTime.UtcNow);
            auditable.UpdatedBy = auditable.CreatedBy with { }; // shallow copy
        }
    }

    public static void SetUpdateStamp(object instance, IUserContextService userContextService)
    {
        if (instance is IAuditable auditable)
        {
            auditable.UpdatedBy = new UserActionStamp(userContextService.ApplicationUserId!, DateTime.UtcNow);
        }
    }
}