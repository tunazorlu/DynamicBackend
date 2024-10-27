using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Shared.DTOs;




public class CreateTableDto : INotifyPropertyChanged
{
    private string tableName = string.Empty;
    private List<CreateColumnDto> columns = new();

    public string TableName
    {
        get => tableName;
        set
        {
            if (tableName != value)
            {
                tableName = value;
                OnPropertyChanged();
            }
        }
    }

    public List<CreateColumnDto> Columns
    {
        get => columns;
        set
        {
            if (columns != value)
            {
                columns = value;
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
