using System.ComponentModel;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shared.DTOs;

public class CreateTableDto : INotifyPropertyChanged
{
    public int Id { get; set; }
    private string _tableName = string.Empty;
    private List<CreateColumnDto> _columns = new();

    public string TableName
    {
        get => _tableName;
        set
        {
            if (_tableName != value)
            {
                _tableName = value;
                OnPropertyChanged();
            }
        }
    }

    public List<CreateColumnDto> Columns
    {
        get => _columns;
        set
        {
            if (_columns != value)
            {
                _columns = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}