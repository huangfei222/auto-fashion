using Genesis.Engine.Core.Config;


public static class TestConfig
{

    public static void Load(
        ConfigManager config
    )
    {


        config.Register
        (
            "1001",

            new Dictionary<string,object>
            {

                {
                    "type",
                    "Runtime"
                }

            }

        );

    }

}