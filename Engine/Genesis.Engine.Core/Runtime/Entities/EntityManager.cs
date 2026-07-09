using Genesis.Engine.Core.Logging;


namespace Genesis.Engine.Core.Runtime.Entities;


public class EntityManager
{

    private readonly List<Entity> entities = new();


    public void Add(Entity entity)
    {
        entities.Add(entity);

        Logger.Info(
            $"Entity Added {entity.Id.Value}"
        );
    }


    public int Count()
    {
        return entities.Count;
    }

}