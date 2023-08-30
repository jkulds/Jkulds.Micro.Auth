namespace Jkulds.Micro.Base;

public abstract class ModelBase
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}