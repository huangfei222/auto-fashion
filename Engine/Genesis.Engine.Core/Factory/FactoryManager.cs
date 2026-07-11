namespace Genesis.Engine.Core.Factory;


public class FactoryManager
{

    private readonly Dictionary<Type, object> factories = new();


    public void Register<T>(
        IFactory<T> factory
    )
    {
        factories[typeof(T)] = factory;
    }


    public IFactory<T> Get<T>()
    {
        return 
        (IFactory<T>)
        factories[typeof(T)];
    }


    public bool Exists<T>()
    {
        return factories.ContainsKey(typeof(T));
    }

}