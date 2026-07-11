namespace Genesis.Engine.Core.Runtime.Persistence;


public class SaveData
{

    public string Version { get; set; }

    public List<object> Entities { get; set; }


    public SaveData()
    {
        Version = "0.1";
        Entities = new List<object>();
    }

}