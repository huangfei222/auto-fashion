using Genesis.Engine.Core.Bootstrap;
using Genesis.Engine.Core.Logging;
using Genesis.Engine.Core.Runtime.Entities;
using Genesis.Engine.Core.Runtime.Components;



var engine =
new EngineBootstrap();



engine.Start();



var entity =
new Entity
(
    new EntityId(3001),
    "RuntimeObject"
);



engine.Entities.Add(entity);



var data =
new RuntimeDataComponent(
    "ConfigDriven"
);



engine.Components.Add(
    entity,
    data
);



var result =
engine.Components.Get<RuntimeDataComponent>(
    entity
);



Logger.Info(
    $"Component Value {result.Value}"
);



Logger.Info(
    $"Entity Count {engine.Entities.Count()}"
);



engine.Stop();