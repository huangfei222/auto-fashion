namespace Genesis.Engine.Core.Runtime;


public class RuntimeContext
{


    public bool Running { get; private set; }



    public void Start()
    {

        Running = true;

    }



    public void Update(
        float deltaTime
    )
    {

        if(!Running)
            return;


        // Runtime Loop

    }




    public void Stop()
    {

        Running = false;

    }


}