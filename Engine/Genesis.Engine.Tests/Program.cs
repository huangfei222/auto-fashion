using Genesis.Engine.Core.Bootstrap;
using Genesis.Engine.Core.Entity;
using Genesis.Engine.Core.Logging;



var engine =
new EngineBootstrap();



engine.Start();



TestConfig.Load(
    engine.Config
);



var factory =
engine.Factory.Get<Entity>();



var entity =
factory.Create(1001);



engine.Entities.Add(entity);



Logger.Info(
    $"Entity Type {entity.Type}"
);



Logger.Info(
    $"Entity Count {engine.Entities.Count}"
);



engine.Stop();