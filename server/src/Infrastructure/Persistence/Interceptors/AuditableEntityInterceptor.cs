using StudyZen.Domain.Interfaces;
using StudyZen.Domain.ValueObjects;

namespace StudyZen.Infrastructure.Persistence;

public static class AuditableEntityInterceptor
{
    public static void SetCreateStamp(IAuditable instance)
    {
        instance.CreatedBy = new UserActionStamp(Environment.UserName, DateTime.UtcNow);
        SetUpdateStamp(instance);
    }

    public static void SetUpdateStamp(IAuditable instance)
    {
        instance.UpdatedBy = new UserActionStamp(Environment.UserName, DateTime.UtcNow);
    }
}