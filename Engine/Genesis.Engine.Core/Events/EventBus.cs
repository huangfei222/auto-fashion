using System;
using System.Collections.Generic;


namespace Genesis.Engine.Core.Events;


public class EventBus
{

    private readonly Dictionary<
        string,
        List<Action<object>>
    > listeners = new();



    public void Subscribe(
        string eventName,
        Action<object> handler
    )
    {

        if(!listeners.ContainsKey(eventName))
        {
            listeners[eventName]
            =
            new List<Action<object>>();
        }


        listeners[eventName]
        .Add(handler);

    }



    public void Emit(
        string eventName,
        object data = null
    )
    {

        if(!listeners.ContainsKey(eventName))
        {
            return;
        }



        foreach(
            var handler 
            in listeners[eventName]
        )
        {

            handler(data);

        }

    }



    public void Remove(
        string eventName,
        Action<object> handler
    )
    {

        if(
            listeners.ContainsKey(eventName)
        )
        {

            listeners[eventName]
            .Remove(handler);

        }

    }

}