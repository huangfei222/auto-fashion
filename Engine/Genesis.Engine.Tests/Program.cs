using Genesis.Engine.Core.Bootstrap;
using Genesis.Engine.Core.Logging;

using Genesis.Engine.Core.Runtime.Entities;
using Genesis.Engine.Core.Runtime.Components;
using Genesis.Engine.Core.Runtime.Systems;

using Genesis.Engine.Core.Runtime.Resource;
using Genesis.Engine.Core.Runtime.Serialization;
using Genesis.Engine.Core.Runtime.Data;

using Genesis.Engine.Core.Runtime;
using Genesis.Engine.Core.Events;
using Genesis.Engine.Core.Runtime.Modules;

var engine = new EngineBootstrap();

try
{
    engine.Start();

    ServiceContainerTest.Run();

    PersistenceTest.Run(engine.Services);

    // ===============================
    // Resource System Test
    // ===============================
    ResourceManager? resourceManager = null;
    if (!engine.Services.TryResolve<ResourceManager>(out resourceManager) || resourceManager == null)
    {
        resourceManager = new ResourceManager();
        Logger.Warn("Program: ResourceManager not registered in container; using local instance.");
    }


    var runtimeResource = new RuntimeConfigResource("runtime.test", "Resource Loaded");

    resourceManager.Register(runtimeResource);

    var loadedResource = resourceManager.Get<RuntimeConfigResource>("runtime.test");

    if (loadedResource is not null)
    {
        Logger.Info(loadedResource.Value);
    }

    // ===============================
    // Service Container Test
    // ===============================
    if (engine.Services.TryResolve<EventBus>(out var eventBus) && eventBus != null)
    {
        Logger.Info("Service Container OK");
    }
    else
    {
        Logger.Warn("Program: EventBus not registered in container.");
    }

    // ===============================
    // Serialization Test
    // ===============================
    var serializer = new JsonSerializer<RuntimeData>();

    var runtimeData = new RuntimeData
    {
        Id = 3001,
        Type = "Runtime",
        Value = 500
    };

    var jsonData = serializer.Serialize(runtimeData);

    Logger.Info(jsonData);

    var deserializeData = serializer.Deserialize(jsonData);

    if (deserializeData is not null)
    {
        Logger.Info($"Deserialize {deserializeData.Id}");
    }

    // ===============================
    // Data Pipeline Test
    // ===============================
    var loader = new JsonDataLoader<RuntimeData>();

    var loadedRuntime = loader.Load("Data/runtime.json");

    Logger.Info($"Loaded Data {loadedRuntime.Id}");
    Logger.Info($"Type {loadedRuntime.Type}");
    Logger.Info($"Value {loadedRuntime.Value}");

    // ===============================
    // Entity System Test
    // ===============================
    var entity = new Entity(new EntityId(3001), "RuntimeObject");

    if (engine.Services.TryResolve<EntityManager>(out var entityManager) && entityManager != null)
    {
        entityManager.Add(entity);
    }
    else
    {
        Logger.Warn("Program: EntityManager not registered; skipping entity add.");
    }

    // ===============================
    // Component System Test
    // ===============================
    var runtimeComponent = new RuntimeDataComponent("ConfigDriven");

    if (engine.Services.TryResolve<ComponentManager>(out var componentManager) && componentManager != null)
    {
        componentManager.Add(entity, runtimeComponent);
    }
    else
    {
        Logger.Warn("Program: ComponentManager not registered; skipping component add.");
    }

    // ===============================
    // System Test
    // ===============================
    if (engine.Services.TryResolve<SystemManager>(out var systemManager) && systemManager != null)
    {
        systemManager.Add<RuntimeTestSystem>();
    }
    else
    {
        Logger.Warn("Program: SystemManager not registered; skipping system registration.");
    }

    // ===============================
    // Runtime Loop
    // ===============================
    if (engine.Services.TryResolve<EngineLoop>(out var loop) && loop != null)
    {
        try
        {
            loop.Run();
        }
        catch (Exception ex)
        {
            Logger.Warn($"Program: EngineLoop.Run threw: {ex.Message}");
        }
    }
    else
    {
        Logger.Warn("Program: EngineLoop not registered; skipping runtime loop.");
    }

    // ===============================
    // Component Verify
    // ===============================
    if (engine.Services.TryResolve<ComponentManager>(out var compMgr) && compMgr != null)
    {
        var loadedComponent = compMgr.Get<RuntimeDataComponent>(entity);
        if (loadedComponent is not null)
        {
            Logger.Info($"Component Value {loadedComponent.Value}");
        }
    }

    if (engine.Services.TryResolve<EntityManager>(out var entMgr) && entMgr != null)
    {
        Logger.Info($"Entity Count {entMgr.Count()}");
    }

}
catch (Exception ex)
{
    Logger.Info($"Unhandled exception: {ex}");
}
finally
{
    engine.Stop();
}
