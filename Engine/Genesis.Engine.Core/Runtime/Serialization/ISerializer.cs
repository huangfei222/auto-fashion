namespace Genesis.Engine.Core.Runtime.Serialization;


public interface ISerializer<T>
{
    string Serialize(T data);


    T Deserialize(string json);
}