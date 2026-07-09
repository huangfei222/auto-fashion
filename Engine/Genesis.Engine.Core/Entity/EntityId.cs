namespace Genesis.Engine.Core.Entity;


public readonly struct EntityId
{

    public int Value { get; }


    public EntityId(int value)
    {
        Value = value;
    }


    public override string ToString()
    {
        return Value.ToString();
    }

}