using System;
using System.Collections.Generic;
using System.Linq;
using Genesis.Engine.Core.Logging;
using Genesis.Engine.Core.Services;

namespace Genesis.Engine.Core.Runtime.Systems
{
    public class SystemManager
    {
        private readonly List<SystemBase> systems = new();
        private readonly ServiceContainer services;

        public SystemManager(ServiceContainer services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public void Add<T>() where T : SystemBase, new()
        {
            try
            {
                var system = new T();
                systems.Add(system);
                system.Initialize();
                Logger.Info($"SystemManager: Added system {typeof(T).Name}");
            }
            catch (Exception ex)
            {
                Logger.Warn($"SystemManager: Failed to add system {typeof(T).Name}: {ex.Message}");
            }
        }

        public void Add(SystemBase system)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));
            try
            {
                systems.Add(system);
                system.Initialize();
                Logger.Info($"SystemManager: Added system {system.GetType().Name}");
            }
            catch (Exception ex)
            {
                Logger.Warn($"SystemManager: Failed to add system {system.GetType().Name}: {ex.Message}");
            }
        }

        public void Update(float deltaTime)
        {
            foreach (var system in systems.ToList())
            {
                try
                {
                    system.Update(deltaTime);
                }
                catch (Exception ex)
                {
                    Logger.Warn($"SystemManager: System {system.GetType().Name} update error: {ex.Message}");
                }
            }
        }

        public void Shutdown()
        {
            foreach (var system in systems.ToList())
            {
                try
                {
                    system.Shutdown();
                }
                catch (Exception ex)
                {
                    Logger.Warn($"SystemManager: System {system.GetType().Name} shutdown error: {ex.Message}");
                }
            }

            systems.Clear();
            Logger.Info("SystemManager: All systems shutdown.");
        }
    }
}
