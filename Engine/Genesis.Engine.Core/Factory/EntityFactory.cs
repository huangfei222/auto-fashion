using Genesis.Engine.Core.Config;
using Genesis.Engine.Core.Entity;


namespace Genesis.Engine.Core.Factory;


public class EntityFactory : IFactory<Entity>
{

    private readonly ConfigManager config;


    public EntityFactory(
        ConfigManager config
    )
    {
        this.config = config;
    }



    public Entity Create(
        int id
    )
    {

        var data =
        config.Get<Dictionary<string,object>>
        (
            id.ToString()
        );


        return new Entity
        (
            new EntityId(id),
            data["type"].ToString()
        );

    }

}