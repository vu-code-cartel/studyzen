using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using StudyZen.Domain.Interfaces;
using StudyZen.Domain.ValueObjects;

namespace StudyZen.Infrastructure.Persistence;

public static class AuditableEntityInterceptor
{
    public static void SetCreateStamp(object instance, IHttpContextAccessor httpContextAccessor)
    {
        if (instance is IAuditable auditable)
        {
            var applicationUserId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? throw new InvalidOperationException("Unable to retrieve user identity.");

            auditable.CreatedBy = new UserActionStamp(applicationUserId, DateTime.UtcNow);
            auditable.UpdatedBy = auditable.CreatedBy with { }; // shallow copy
        }
    }

    public static void SetUpdateStamp(object instance, IHttpContextAccessor httpContextAccessor)
    {
        if (instance is IAuditable auditable)
        {
            var applicationUserId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? throw new InvalidOperationException("Unable to retrieve user identity.");

            auditable.UpdatedBy = new UserActionStamp(applicationUserId, DateTime.UtcNow);
        }
    }
}