using Genesis.Engine.Core.Logging;
using Genesis.Engine.Core.Runtime.Systems;


namespace Genesis.Engine.Core.Runtime;


public class EngineLoop
{

    private readonly SystemManager systems;


    private bool running;


    public EngineLoop(
        SystemManager systems
    )
    {
        this.systems = systems;
    }



    public void Run(
        int frames = 3
    )
    {

        running = true;


        Logger.Info(
            "Engine Loop Started"
        );


        for(
            int i = 0;
            i < frames && running;
            i++
        )
        {

            Tick(
                0.016f
            );

        }


        Stop();

    }



    private void Tick(
        float deltaTime
    )
    {

        systems.Update(
            deltaTime
        );


    }



    public void Stop()
    {

        running=false;


        Logger.Info(
            "Engine Loop Stopped"
        );

    }

}