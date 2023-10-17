using StudyZen.Domain.Interfaces;
using StudyZen.Domain.ValueObjects;

namespace StudyZen.Infrastructure.Persistence;

public static class AuditableEntityInterceptor
{
    public static void SetCreateStamp(object instance)
    {
        if (instance is IAuditable auditable)
        {
            auditable.CreatedBy = new UserActionStamp(Environment.UserName, DateTime.UtcNow);
            auditable.UpdatedBy = auditable.CreatedBy with { };
        }
    }

    public static void SetUpdateStamp(object instance)
    {
        if (instance is IAuditable auditable)
        {
            auditable.UpdatedBy = new UserActionStamp(Environment.UserName, DateTime.UtcNow);
        }
    }
}