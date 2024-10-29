using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Shared.DTOs;

public class CreateColumnDto : INotifyPropertyChanged
{
    private bool _isPrimaryKey;
    private string _name;
    private string _dataType;
    private bool _isNullable;

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    public string DataType
    {
        get => _dataType;
        set
        {
            if (_dataType != value)
            {
                _dataType = value;
                OnPropertyChanged(nameof(DataType));
            }
        }
    }

    public bool IsNullable
    {
        get => _isNullable;
        set
        {
            if (_isNullable != value)
            {
                _isNullable = value;
                OnPropertyChanged(nameof(IsNullable));
            }
        }
    }

    public bool IsPrimaryKey
    {
        get => _isPrimaryKey;
        set
        {
            if (_isPrimaryKey != value)
            {
                _isPrimaryKey = value;
                OnPropertyChanged(nameof(IsPrimaryKey));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

