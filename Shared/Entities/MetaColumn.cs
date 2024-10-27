namespace Shared.Entities;

public class MetaColumn
{
    public int Id { get; set; }
    public int MetaTableId { get; set; }
    public string Name { get; set; }
    public string DataType { get; set; }
    public bool IsNullable { get; set; }
    public bool IsPrimaryKey { get; set; }
    public int Order { get; set; }
    public MetaTable MetaTable { get; set; }
}