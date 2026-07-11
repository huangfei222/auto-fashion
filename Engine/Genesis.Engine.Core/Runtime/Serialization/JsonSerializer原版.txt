using System.Text.Json;


namespace Genesis.Engine.Core.Runtime.Serialization;


public class JsonSerializer<T>
    :
    ISerializer<T>
{

    public string Serialize(
        T data
    )
    {

        return JsonSerializer.Serialize(
            data
        );

    }



    public T Deserialize(
        string data
    )
    {

        var result =
            System.Text.Json.JsonSerializer.Deserialize<T>(
                data
            );


        if(result == null)
        {
            throw new InvalidOperationException(
                "Deserialize returned null"
            );
        }


        return result;

    }

}