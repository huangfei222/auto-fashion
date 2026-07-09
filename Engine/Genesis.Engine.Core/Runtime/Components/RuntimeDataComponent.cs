namespace Genesis.Engine.Core.Runtime.Components;


public class RuntimeDataComponent 
    : Component
{

    public string Value {get;}


    public RuntimeDataComponent(
        string value
    )
    {
        Value = value;
    }

}