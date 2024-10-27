using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Shared.DTOs;

public class CreateColumnDto : INotifyPropertyChanged
{
    private string name = string.Empty;
    private string dataType = string.Empty;
    private bool isNullable;
    private bool isPrimaryKey;

    public string Name
    {
        get => name;
        set
        {
            if (name != value)
            {
                name = value;
                OnPropertyChanged();
            }
        }
    }

    public string DataType
    {
        get => dataType;
        set
        {
            if (dataType != value)
            {
                dataType = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsNullable
    {
        get => isNullable;
        set
        {
            if (isNullable != value)
            {
                isNullable = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsPrimaryKey
    {
        get => isPrimaryKey;
        set
        {
            if (isPrimaryKey != value)
            {
                isPrimaryKey = value;
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