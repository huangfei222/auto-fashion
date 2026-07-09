using System.Text.Json;


namespace Genesis.Engine.Core.Runtime.Data;


public class JsonDataLoader<T>
    :
    IDataLoader<T>
{

    public T Load(
        string path
    )
    {

        var json =
            File.ReadAllText(path);



        var result =
            JsonSerializer.Deserialize<T>(
                json
            );



        if(result == null)
        {
            throw new Exception(
                "Data load failed"
            );
        }



        return result;

    }

}