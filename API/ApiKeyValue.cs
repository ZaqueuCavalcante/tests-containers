namespace API;

public class ApiKeyValue
{
    public string Key { get; set; }
    public string Value { get; set; }

    public ApiKeyValue(string key, string value)
    {
        Key = key;
        Value = value;
    }
}
