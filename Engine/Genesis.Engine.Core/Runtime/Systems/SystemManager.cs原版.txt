namespace Genesis.Engine.Core.Runtime;


public class SystemManager
{


    private readonly List<SystemBase> systems
    =
    new();



    public void Add(
        SystemBase system
    )
    {

        systems.Add(system);

    }



    public void Initialize()
    {

        foreach(
            var system in systems
        )
        {

            system.Initialize();

        }

    }




    public void Update(
        float delta
    )
    {

        foreach(
            var system in systems
        )
        {

            system.Update(delta);

        }

    }



    public void Shutdown()
    {

        foreach(
            var system in systems
        )
        {

            system.Shutdown();

        }

    }


}
