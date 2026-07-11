namespace Genesis.Engine.Core.Runtime.Modules;


public class ModuleDefinition
{


    public string Type { get; set; }


    public bool Enabled { get; set; }


    public int Order { get; set; }



    public ModuleDefinition()
    {

        Type = "";

        Enabled = true;

        Order = 0;

    }


}