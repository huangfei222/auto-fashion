using Genesis.Engine.Core.Bootstrap;
using Genesis.Engine.Core.Logging;
using Genesis.Engine.Core.Runtime.Entities;



var engine =
new EngineBootstrap();



engine.Start();



engine.EventBus.Subscribe(
    "EntityCreated",
    data =>
    {

        var entity =
        (Entity)data;


        Logger.Info(
            $"Created Entity {entity.Id.Value}"
        );

    }
);



var entity =
new Entity
(
    new EntityId(2001),
    "RuntimeObject"
);



engine.Entities.Add(entity);



Logger.Info(
    $"Total Entity {engine.Entities.Count()}"
);



engine.Stop();