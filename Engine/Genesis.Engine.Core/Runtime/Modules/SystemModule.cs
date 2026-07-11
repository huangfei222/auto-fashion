using System;
using Genesis.Engine.Core.Services;
using Genesis.Engine.Core.Logging;

namespace Genesis.Engine.Core.Runtime.Modules
{
    public class SystemModule : IEngineModule
    {
        public string Name => "SystemModule";
        public int Order => 20;
        public void Register(ServiceContainer services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (services.TryResolve<Genesis.Engine.Core.Runtime.Systems.SystemManager>(out var existing))
            {
                Logger.Info("SystemModule: SystemManager already registered in container.");
                return;
            }

            try
            {
                var sysMgrType = typeof(Genesis.Engine.Core.Runtime.Systems.SystemManager);
                var ctor = sysMgrType.GetConstructor(new Type[] { typeof(ServiceContainer) });
                object sysMgrInstance;
                if (ctor != null)
                {
                    sysMgrInstance = ctor.Invoke(new object[] { services });
                }
                else
                {
                    var parameterless = sysMgrType.GetConstructor(Type.EmptyTypes);
                    if (parameterless != null)
                    {
                        sysMgrInstance = Activator.CreateInstance(sysMgrType)!;
                    }
                    else
                    {
                        var ctors = sysMgrType.GetConstructors();
                        object? created = null;
                        foreach (var c in ctors)
                        {
                            var ps = c.GetParameters();
                            var args = new object?[ps.Length];
                            var ok = true;
                            for (int i = 0; i < ps.Length; i++)
                            {
                                var pType = ps[i].ParameterType;
                                if (pType == typeof(ServiceContainer) || pType.IsAssignableFrom(typeof(ServiceContainer)))
                                {
                                    args[i] = services;
                                    continue;
                                }
                                if (services.TryResolve(pType, out var resolved))
                                {
                                    args[i] = resolved;
                                    continue;
                                }
                                ok = false;
                                break;
                            }
                            if (!ok) continue;
                            try { created = c.Invoke(args); break; } catch { created = null; }
                        }

                        if (created == null)
                        {
                            throw new InvalidOperationException("SystemModule: Cannot construct SystemManager - no suitable constructor found.");
                        }

                        sysMgrInstance = created;
                    }
                }

                services.Register(typeof(Genesis.Engine.Core.Runtime.Systems.SystemManager), sysMgrInstance!);
                Logger.Info("SystemModule: SystemManager registered.");
            }
            catch (Exception ex)
            {
                Logger.Warn($"SystemModule: Failed to register SystemManager: {ex.Message}");
            }
        }

        public void Initialize()
        {
            Logger.Info("SystemModule: Initialize called.");
        }

        public void Shutdown()
        {
            Logger.Info("SystemModule: Shutdown called.");
        }
    }
}
