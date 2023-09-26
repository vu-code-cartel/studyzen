namespace StudyZen.Infrastructure.Repositories;

internal sealed class EntitySet<TEntity>
{
    public int TotalCount { get; set; }
    public List<TEntity> Instances { get; } = new();
}