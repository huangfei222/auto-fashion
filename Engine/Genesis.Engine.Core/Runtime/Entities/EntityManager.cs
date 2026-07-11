using Genesis.Engine.Core.Events;


namespace Genesis.Engine.Core.Runtime.Entities;


public class EntityManager
{

    private readonly Dictionary<int, Entity> entities = new();


    private readonly EventBus eventBus;



    public EntityManager(
        EventBus eventBus
    )
    {
        this.eventBus = eventBus;
    }



    public void Add(
        Entity entity
    )
    {

        entities[entity.Id.Value]
        =
        entity;


        eventBus.Emit(
            "EntityCreated",
            entity
        );

    }



    public Entity Get(
        int id
    )
    {
        return entities[id];
    }



    public int Count()
    {
        return entities.Count;
    }



    public void Remove(
        int id
    )
    {

        if(
            entities.ContainsKey(id)
        )
        {

            var entity =
            entities[id];


            entities.Remove(id);


            eventBus.Emit(
                "EntityRemoved",
                entity
            );

        }

    }

}