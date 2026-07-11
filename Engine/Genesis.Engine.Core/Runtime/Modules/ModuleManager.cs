using System;
using System.Collections.Generic;
using System.Linq;
using Genesis.Engine.Core.Logging;
using Genesis.Engine.Core.Services;

namespace Genesis.Engine.Core.Runtime.Modules
{
    /// <summary>
    /// ModuleManager
    /// - 管理模块的 Register/Initialize/Shutdown 生命周期
    /// - 在 Register 时把模块需要的服务注册到 ServiceContainer
    /// </summary>
    public class ModuleManager
    {
        private readonly List<IEngineModule> modules = new();
        private readonly ServiceContainer services;

        public ModuleManager(ServiceContainer services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public void Add(IEngineModule module)
        {
            if (module == null) throw new ArgumentNullException(nameof(module));
            modules.Add(module);
            // Register module services immediately so other modules can depend on them during Initialize
            try
            {
                module.Register(services);
                Logger.Info($"ModuleManager: Registered module {module.Name}");
            }
            catch (Exception ex)
            {
                Logger.Warn($"ModuleManager: Module {module.Name} Register failed: {ex.Message}");
            }
        }

        public void Initialize()
        {
            foreach (var m in modules.OrderBy(m => m.Order))
            {
                try
                {
                    m.Initialize();
                    Logger.Info($"ModuleManager: Initialized module {m.Name}");
                }
                catch (Exception ex)
                {
                    Logger.Warn($"ModuleManager: Module {m.Name} Initialize failed: {ex.Message}");
                }
            }
        }

        public void Shutdown()
        {
            foreach (var m in modules.OrderByDescending(m => m.Order))
            {
                try
                {
                    m.Shutdown();
                    Logger.Info($"ModuleManager: Shutdown module {m.Name}");
                }
                catch (Exception ex)
                {
                    Logger.Warn($"ModuleManager: Module {m.Name} Shutdown failed: {ex.Message}");
                }
            }
            modules.Clear();
        }

        public IReadOnlyList<IEngineModule> GetModules() => modules.AsReadOnly();
    }
}
