namespace Genesis.Engine.Core.Runtime.Entities;


public readonly struct EntityId
{

    public int Value { get; }


    public EntityId(
        int value
    )
    {
        Value = value;
    }


}