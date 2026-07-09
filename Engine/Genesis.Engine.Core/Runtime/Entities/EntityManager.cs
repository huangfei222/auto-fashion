namespace Genesis.Engine.Core.Runtime.Entity;


public class EntityManager
{


    private int counter=0;



    private readonly Dictionary<int,Entity>
    entities
    =
    new();



    public Entity Create()
    {


        counter++;


        var entity =
        new Entity(
            new EntityId(counter)
        );


        entities[counter]=entity;


        return entity;

    }




    public Entity Get(
        int id
    )
    {


        return entities[id];


    }


}
