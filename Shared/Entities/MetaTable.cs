namespace Shared.Entities;

public class MetaTable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public List<MetaColumn> Columns { get; set; }
}