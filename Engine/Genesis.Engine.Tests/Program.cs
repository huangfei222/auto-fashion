using Genesis.Engine.Core.Bootstrap;
using Genesis.Engine.Core.Runtime.Entities;
using Genesis.Engine.Core.Logging;

var engine = new EngineBootstrap();

engine.Start();

var entityManager = new EntityManager();

var entity = new Entity
(
    new EntityId(1001),
    "Runtime"
);

entityManager.Add(entity);

Logger.Info($"Entity Count {entityManager.Count()}");

engine.Stop();