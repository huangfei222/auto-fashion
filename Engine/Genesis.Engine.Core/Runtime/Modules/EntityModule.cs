using Genesis.Engine.Core.Services;
using Genesis.Engine.Core.Events;
using Genesis.Engine.Core.Runtime.Entities;


namespace Genesis.Engine.Core.Runtime.Modules;



public class EntityModule
    : IEngineModule
{


    public string Name
    {
        get
        {
            return "EntityModule";
        }
    }



    public int Order
    {
        get
        {
            return 1;
        }
    }






    public void Register(
        ServiceContainer services
    )
    {


        var eventBus =
        services.Get<EventBus>();



        services.Register(
            new EntityManager(
                eventBus
            )
        );


    }







    public void Initialize()
    {

    }







    public void Shutdown()
    {

    }


}