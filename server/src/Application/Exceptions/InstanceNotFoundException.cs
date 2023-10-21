namespace StudyZen.Application.Exceptions;

public sealed class InstanceNotFoundException : Exception
{
    public InstanceNotFoundException(string entity, int instanceId) 
        : base($"Could not find an instance of '{entity}' by id {instanceId}")
    {
    }
}