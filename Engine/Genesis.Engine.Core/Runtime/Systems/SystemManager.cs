namespace Genesis.Engine.Core.Runtime.Systems;


public class SystemManager
{

    private readonly List<SystemBase> systems
        =
        new();



    public void Add<T>()
        where T:SystemBase,new()
    {

        var system =
        new T();


        systems.Add(system);


        system.Initialize();

    }




    public void Update(
        float deltaTime
    )
    {

        foreach(
            var system 
            in systems
        )
        {

            system.Update(
                deltaTime
            );

        }

    }



    public void Shutdown()
    {

        foreach(
            var system 
            in systems
        )
        {

            system.Shutdown();

        }


        systems.Clear();

    }

}