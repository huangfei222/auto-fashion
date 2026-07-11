namespace Genesis.Engine.Core.Factory;


public interface IFactory<T>
{

    T Create(
        int id
    );

}