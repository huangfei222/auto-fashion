using Genesis.Engine.Core.Bootstrap;
using Genesis.Engine.Core.Logger;



var engine =
new EngineBootstrap();



engine.Start();



Logger.Info(
    "Runtime Test Running"
);



engine.Update(
    0.016f
);



engine.Stop();
