using Newtonsoft.Json;

public class DynamicProperty
{
    public int Id { get; set; }
    public string Name { get; set; }

    private object _value;

    public object Value
    {
        get => _value;
        set => _value = JsonConvert.SerializeObject(value); // Serialize ediyoruz
    }

    public T GetValue<T>()
    {
        return JsonConvert.DeserializeObject<T>(_value.ToString()); // Deserialize ederken doğru türü döndürür
    }
}
