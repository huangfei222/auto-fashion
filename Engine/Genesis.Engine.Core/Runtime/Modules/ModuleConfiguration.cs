namespace Genesis.Engine.Core.Runtime.Modules;


public class ModuleConfiguration
{

    public List<ModuleDefinition> Modules
    {
        get;
        set;
    }


    public ModuleConfiguration()
    {

        Modules =
        new List<ModuleDefinition>();

    }

}