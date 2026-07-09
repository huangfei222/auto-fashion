namespace Genesis.Engine.Core.Runtime.Resource;


public class ResourceHandle<T>
    where T:IResource
{

    public T Resource
    {
        get;
    }



    public ResourceHandle(
        T resource
    )
    {

        Resource = resource;

    }

}