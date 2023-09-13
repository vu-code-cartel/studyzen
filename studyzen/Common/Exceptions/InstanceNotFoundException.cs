namespace StudyZen.Common.Exceptions;

public sealed class InstanceNotFoundException : Exception
{
    public InstanceNotFoundException(string entityName, int id)
        : base($"Could not find an instance of '{entityName}' by id {id}.")
    {
    }
}