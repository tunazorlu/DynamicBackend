namespace Shared.Entities;

public class BaseEntity
{
    public int Id { get; set; }
    public List<DynamicProperty<object>> Properties { get; set; } = new List<DynamicProperty<object>>();
}
