using Genesis.Engine.Core.Logging;
using Genesis.Engine.Core.Runtime.Systems;


public class RuntimeTestSystem
    : SystemBase
{

    public override void Initialize()
    {

        Logger.Info(
            "Runtime System Initialized"
        );

    }



    public override void Update(
        float deltaTime
    )
    {

        Logger.Info(
            $"System Tick {deltaTime}"
        );

    }



    public override void Shutdown()
    {

        Logger.Info(
            "Runtime System Shutdown"
        );

    }

}