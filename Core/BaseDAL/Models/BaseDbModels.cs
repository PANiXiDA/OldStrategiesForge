namespace BaseDAL.Models;

public class BaseDbModel<TObjectId>
{
    public TObjectId Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}

