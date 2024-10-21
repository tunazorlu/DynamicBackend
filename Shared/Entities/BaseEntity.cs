namespace Shared.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public List<DynamicProperty> Properties { get; set; } = new List<DynamicProperty>();
    }
}
