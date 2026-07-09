using Genesis.Engine.Core.Bootstrap;
using Genesis.Engine.Core.Logging;
using Genesis.Engine.Core.Runtime.Entities;
using Genesis.Engine.Core.Runtime.Components;
using Genesis.Engine.Core.Runtime.Resource;
using Genesis.Engine.Core.Runtime.Serialization;


var engine = new EngineBootstrap();


try
{

    engine.Start();



    // ===============================
    // Resource System Test
    // ===============================


    var resourceManager =
        new ResourceManager();



    var runtimeResource =
        new RuntimeConfigResource(
            "runtime.test",
            "Resource Loaded"
        );



    resourceManager.Register(
        runtimeResource
    );



    var loadedResource =
        resourceManager.Get<RuntimeConfigResource>(
            "runtime.test"
        );



    if(loadedResource is not null)
    {
        Logger.Info(
            loadedResource.Value
        );
    }




    // ===============================
    // Serialization Test
    // ===============================


    var serializer =
        new JsonSerializer<RuntimeData>();



    var runtimeData =
        new RuntimeData
        {
            Id = 3001,
            Type = "Runtime",
            Value = 500
        };



    var jsonData =
        serializer.Serialize(
            runtimeData
        );



    Logger.Info(
        jsonData
    );



    var deserializeData =
        serializer.Deserialize(
            jsonData
        );



    if(deserializeData is not null)
    {
        Logger.Info(
            $"Deserialize {deserializeData.Id}"
        );
    }




    // ===============================
    // Entity System Test
    // ===============================


    var entity =
        new Entity(
            new EntityId(3001),
            "RuntimeObject"
        );



    engine.Entities.Add(
        entity
    );



    // 注意：
    // 这里必须使用 Component
    // 不能使用 RuntimeData


    var runtimeComponent =
        new RuntimeDataComponent(
            "ConfigDriven"
        );



    engine.Components.Add(
        entity,
        runtimeComponent
    );




    // ===============================
    // System Test
    // ===============================


    engine.Systems.Add<RuntimeTestSystem>();



    engine.Loop.Run();



    var loadedComponent =
        engine.Components.Get<RuntimeDataComponent>(
            entity
        );



    if(loadedComponent is not null)
    {

        Logger.Info(
            $"Component Value {loadedComponent.Value}"
        );

    }



    Logger.Info(
        $"Entity Count {engine.Entities.Count()}"
    );



}
catch(Exception ex)
{

    Logger.Info(
        $"Unhandled exception: {ex}"
    );

}
finally
{

    engine.Stop();

}