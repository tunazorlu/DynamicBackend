using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Shared.DTOs;

public class CreateColumnDto : INotifyPropertyChanged
{
    private bool _isPrimaryKey;
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
    private string _name = string.Empty;
    private string _dataType = "string";
    private bool _isNullable = true;

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public string DataType
    {
        get => _dataType;
        set => _dataType = value;
    }

    public bool IsNullable
    {
        get => _isNullable;
        set => _isNullable = value;
    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
