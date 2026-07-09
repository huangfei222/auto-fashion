using Genesis.Engine.Core.Runtime.Entities;


namespace Genesis.Engine.Core.Runtime.Components;


public class ComponentManager
{

    private readonly Dictionary<
        int,
        List<Component>
    > components = new();



    public void Add<T>(
        Entity entity,
        T component
    )
    where T : Component
    {

        var id =
        entity.Id.Value;


        if(!components.ContainsKey(id))
        {
            components[id]
            =
            new List<Component>();
        }


        components[id]
        .Add(component);

    }




    public T? Get<T>(
        Entity entity
    )
    where T : Component
    {

        var list =
        components[
            entity.Id.Value
        ];


        foreach(
            var component 
            in list
        )
        {

            if(component is T)
            {
                return (T)component;
            }

        }


        return null;

    }



    public void RemoveEntity(
        Entity entity
    )
    {

        components.Remove(
            entity.Id.Value
        );

    }


}