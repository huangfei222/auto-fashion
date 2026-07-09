using Genesis.Engine.Core.Runtime.Resource;


public class RuntimeConfigResource
    : IResource
{


    public string Key
    {
        get;
    }



    public string Value
    {
        get;
    }



    public RuntimeConfigResource(
        string key,
        string value
    )
    {

        Key = key;

        Value = value;

    }

}