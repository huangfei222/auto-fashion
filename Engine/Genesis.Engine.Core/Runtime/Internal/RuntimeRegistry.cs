namespace Genesis.Engine.Core.Runtime.Services;


public class ServiceContainer
    :
    IServiceContainer
{

    private readonly Dictionary<Type,object>
        services = new();



    public void Register<T>(
        T service
    )
    {

        services[
            typeof(T)
        ]
        =
        service!;

    }



    public T Get<T>()
    {

        return 
        (T)services[
            typeof(T)
        ];

    }

}