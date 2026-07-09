namespace Genesis.Engine.Core.Config;


public class ConfigManager
{

    private readonly Dictionary<string, object> configs = new();


    public void Register<T>(
        string key,
        T data
    )
    {
        if(data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        configs[key] = data;
    }


    public T Get<T>(
        string key
    )
    {
        return (T)configs[key];
    }


    public bool Exists(
        string key
    )
    {
        return configs.ContainsKey(key);
    }

}