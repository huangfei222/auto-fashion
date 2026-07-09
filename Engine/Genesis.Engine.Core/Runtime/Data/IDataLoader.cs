namespace Genesis.Engine.Core.Runtime.Data;


public interface IDataLoader<T>
{

    T Load(
        string path
    );

}