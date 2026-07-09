namespace Genesis.Engine.Core.Runtime.Entities;


public class Entity
{
    public EntityId Id { get; }

    public string Type { get; }


    public Entity(
        EntityId id,
        string type
    )
    {
        Id = id;
        Type = type;
    }
}