using Genesis.Engine.Core.Logging;
using Genesis.Engine.Core.Events;
using Genesis.Engine.Core.Config;
using Genesis.Engine.Core.Factory;
using Genesis.Engine.Core.Entity;
using Genesis.Engine.Core.Runtime;


namespace Genesis.Engine.Core.Bootstrap;


public class EngineBootstrap
{

    public EventBus EventBus { get; }

    public ConfigManager Config { get; }

    public FactoryManager Factory { get; }

    public EntityManager Entities {get;}

    public EntityFactory EntityFactory {get;}

    public RuntimeContext Runtime { get; }


    public EngineBootstrap()
    {
        EventBus = new EventBus();

        Config = new ConfigManager();

        Factory =new FactoryManager();

        Runtime =new RuntimeContext();

        Entities =new EntityManager();

        EntityFactory =new EntityFactory(Config);

        Factory.Register<Entity>(EntityFactory);
    }



    public void Start()
    {

        Logger.Info(
            "Genesis Engine Starting"
        );


        Runtime.Start();


        Logger.Info(
            "Genesis Engine Started"
        );

    }




    public void Update(
        float deltaTime
    )
    {

        Runtime.Update(
            deltaTime
        );

    }



    public void Stop()
    {

        Runtime.Stop();


        Logger.Info(
            "Genesis Engine Stopped"
        );

    }

}