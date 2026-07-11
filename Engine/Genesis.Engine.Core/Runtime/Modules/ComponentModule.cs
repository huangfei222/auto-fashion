using Genesis.Engine.Core.Services;
using Genesis.Engine.Core.Runtime.Components;


namespace Genesis.Engine.Core.Runtime.Modules;



public class ComponentModule
    : IEngineModule
{


    public string Name
    {
        get
        {
            return "ComponentModule";
        }
    }



    public int Order
    {
        get
        {
            return 2;
        }
    }







    public void Register(
        ServiceContainer services
    )
    {


        services.Register(
            new ComponentManager()
        );


    }







    public void Initialize()
    {

    }







    public void Shutdown()
    {

    }


}