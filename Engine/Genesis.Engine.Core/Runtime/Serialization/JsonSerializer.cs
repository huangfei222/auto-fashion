using System.Text.Json;


namespace Genesis.Engine.Core.Runtime.Serialization;


public class JsonSerializer<T>
    : ISerializer<T>
{

    public string Serialize(T data)
    {

        return System.Text.Json.JsonSerializer.Serialize(
            data
        );

    }



    public T Deserialize(string json)
    {

        return System.Text.Json.JsonSerializer.Deserialize<T>(
            json
        )!;

    }

}