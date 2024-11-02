namespace Shared.DTOs;

public class MetaTableDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public List<MetaColumnDto> Columns { get; set; }
}