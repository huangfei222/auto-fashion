using Genesis.Engine.Core.Logging;


namespace Genesis.Engine.Core.Entity;


public class EntityManager
{


    private readonly Dictionary<int,Entity>
        entities = new();



    public void Add(Entity entity)
    {

        entities[
            entity.Id.Value
        ]
        =
        entity;


        Logger.Info(
            $"Entity Added {entity.Id}"
        );

    }



    public Entity Get(
        int id
    )
    {

        return entities[id];

    }



    public void Remove(
        int id
    )
    {

        entities.Remove(id);


        Logger.Info(
            $"Entity Removed {id}"
        );

    }



    public int Count
    {

        get
        {

            return entities.Count;

        }

    }

}