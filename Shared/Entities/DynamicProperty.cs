using Shared.Enums;

public class DynamicProperty
{
    public int Id { get; set; }
    public string Name { get; set; }
    public PropertyType PropertyType { get; set; }

    public string StringValue { get; set; }
    public int IntValue { get; set; }
    public bool BoolValue { get; set; }
    public DateTime DateTimeValue { get; set; }
}