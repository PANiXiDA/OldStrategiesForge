namespace BaseBL.Models;

public abstract class BasePostgresEntity : BaseEntity<int>
{
    protected BasePostgresEntity(int id, DateTime createdAt, DateTime updateAt, DateTime? deletedAt) : base(id, createdAt, updateAt, deletedAt) { }

    protected BasePostgresEntity() : base(0, DateTime.UtcNow, DateTime.UtcNow, null) { }
}

