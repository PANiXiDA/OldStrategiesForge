namespace BaseBL.Models;

public abstract class BaseEntity<IdType>
{
    public IdType Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public BaseEntity(IdType id)
    {
        Id = id;
    }

    protected BaseEntity(IdType id, DateTime createdAt, DateTime updateAt, DateTime? deletedAt)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updateAt;
        DeletedAt = deletedAt;
    }
}

