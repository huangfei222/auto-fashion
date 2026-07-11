using Genesis.Engine.Core.Services;
using Genesis.Engine.Core.Logging;


public static class ServiceContainerTest
{

    public static void Run()
    {

        var container =
        new ServiceContainer();



        container.Register(
            "Service Test"
        );



        var result =
        container.Get<string>();



        Logger.Info(
            result
        );


    }

}