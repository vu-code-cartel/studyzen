namespace StudyZen.Persistence;

public sealed class EntitySet<TEntity>
{
    public int TotalCount { get; set; }
    public List<TEntity> Instances { get; } = new();
}