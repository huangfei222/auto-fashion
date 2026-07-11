namespace Genesis.Engine.Core.Runtime.Services;


public interface IServiceContainer
{

    void Register<T>(
        T service
    );


    T Get<T>();

}