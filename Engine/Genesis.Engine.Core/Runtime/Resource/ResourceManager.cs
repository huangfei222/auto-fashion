namespace Genesis.Engine.Core.Runtime.Resource;


public class ResourceManager
{

    private readonly Dictionary
    <
    string,
    object
    >
    resources
    =
    new();



    public void Register<T>(
        T resource
    )
    where T:IResource
    {

        resources[
            resource.Key
        ]
        =
        resource;

    }




    public T Get<T>(
        string key
    )
    where T:IResource
    {

        return
        (T)resources[key];

    }

}