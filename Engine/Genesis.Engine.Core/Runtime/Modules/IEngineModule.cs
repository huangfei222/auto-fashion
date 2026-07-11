namespace Genesis.Engine.Core.Runtime.Modules
{
    /// <summary>
    /// 模块接口
    /// - Register(ServiceContainer) 在引导阶段注册模块所需服务
    /// - Initialize() 在模块加载完成后调用
    /// - Shutdown() 在引擎停止时调用
    /// </summary>
    public interface IEngineModule
    {
        string Name { get; }
        int Order { get; }
        void Register(Genesis.Engine.Core.Services.ServiceContainer services);
        void Initialize();
        void Shutdown();
    }
}
