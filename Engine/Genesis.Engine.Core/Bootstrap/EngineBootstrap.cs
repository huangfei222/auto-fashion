using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Genesis.Engine.Core.Logging;
using Genesis.Engine.Core.Services;
using Genesis.Engine.Core.Events;
using Genesis.Engine.Core.Config;
using Genesis.Engine.Core.Factory;

using Genesis.Engine.Core.Runtime;
using Genesis.Engine.Core.Runtime.Persistence;
using Genesis.Engine.Core.Runtime.Serialization;

using Genesis.Engine.Core.Runtime.Modules;
using Genesis.Engine.Core.Runtime.Data;
using Genesis.Engine.Core.Runtime.Systems;

namespace Genesis.Engine.Core.Bootstrap
{
    /// <summary>
    /// EngineBootstrap - 配置优先，向后兼容
    /// </summary>
    public class EngineBootstrap
    {
        public ServiceContainer Services { get; }

        public ModuleManager Modules { get; }

        public EngineConfiguration? Configuration { get; private set; }

        private readonly string configPath;

        public EngineBootstrap(string configPath = "Data/Genesis.Engine.Config.json")
        {
            this.configPath = configPath ?? throw new ArgumentNullException(nameof(configPath));

            Services = new ServiceContainer();

            // 最低限度先注册 ConfigManager，供后续反射/加载使用
            Services.Register(new ConfigManager());

            try
            {
                LoadConfigurationAndServices();
            }
            catch (Exception ex)
            {
                Logger.Warn($"EngineBootstrap: Configuration-driven startup failed: {ex.Message}. Falling back to legacy registration.");
                RegisterBaseServices();
            }

            Modules = new ModuleManager(Services);

            RegisterModules();

            Modules.Initialize();
        }

        private void LoadConfigurationAndServices()
        {
            // 尽量安全获取 ConfigManager
            if (!Services.TryResolve<ConfigManager>(out var configManager) || configManager == null)
            {
                throw new InvalidOperationException("EngineBootstrap: ConfigManager not registered.");
            }

            var engineConfig = configManager.LoadEngineConfiguration(configPath);
            if (engineConfig == null)
            {
                throw new Exception($"EngineBootstrap: Loaded EngineConfiguration is null: {configPath}");
            }
            Configuration = engineConfig;

            Logger.Info($"EngineBootstrap: Loaded EngineConfiguration from {configPath}");

            // Ensure a SerializationManager exists before ServiceLoader runs.
            // Try flexible ctor first, otherwise register a default instance.
            if (!Services.TryResolve<SerializationManager>(out var _))
            {
                if (!InstantiateAndRegister(typeof(SerializationManager)))
                {
                    Services.Register(new SerializationManager());
                    Logger.Info("EngineBootstrap: Registered fallback SerializationManager before ServiceLoader.");
                }
            }

            // 尝试通过反射创建并执行 ServiceLoader（支持多种构造器签名）
            var serviceLoaderType = Type.GetType("Genesis.Engine.Core.Services.ServiceLoader, Genesis.Engine.Core");
            if (serviceLoaderType == null)
            {
                Logger.Warn("EngineBootstrap: ServiceLoader type not found; falling back to RegisterBaseServices.");
                throw new Exception("ServiceLoader type not found");
            }

            object? loaderInstance = null;
            try
            {
                loaderInstance = CreateInstanceWithFlexibleCtor(serviceLoaderType, configManager, Services)
                                ?? CreateInstanceWithFlexibleCtor(serviceLoaderType, Services, configManager);

                if (loaderInstance == null)
                    throw new Exception("ServiceLoader: No suitable constructor found.");

                var loadMethod = serviceLoaderType.GetMethod("LoadServices", BindingFlags.Public | BindingFlags.Instance);
                if (loadMethod == null)
                    throw new Exception("ServiceLoader: LoadServices method not found.");

                // Invoke LoadServices and catch TargetInvocationException to log inner exception details
                try
                {
                    loadMethod.Invoke(loaderInstance, null);
                    Logger.Info("EngineBootstrap: ServiceLoader executed.");
                }
                catch (TargetInvocationException tie)
                {
                    Logger.Warn($"EngineBootstrap: ServiceLoader threw: {tie.InnerException?.Message ?? tie.Message}. Falling back to RegisterBaseServices.");
                    throw new Exception($"ServiceLoader invocation failed: {tie.InnerException?.Message ?? tie.Message}", tie);
                }
            }
            catch (Exception ex)
            {
                // If ServiceLoader fails, surface a clear message and rethrow to be handled by caller
                Logger.Warn($"EngineBootstrap: ServiceLoader invocation failed: {ex.Message}");
                throw;
            }

            // 自动把容器中实现 ISerializer<T> 的实例注册到 SerializationManager（如果存在）
            AutoRegisterSerializersFromContainer();

            // 简单检查：确保至少有除 ConfigManager 之外的服务被注册
            var registeredCount = 0;
            var allDesc = Services.GetAll();
            if (allDesc != null)
            {
                foreach (var d in allDesc)
                {
                    if (d == null) continue;
                    if (d.ServiceType == typeof(ConfigManager)) continue;
                    // Count only successfully registered instances
                    if (d.Instance != null) registeredCount++;
                }
            }

            // Ensure EngineLoop is present after ServiceLoader runs (fix for "EngineLoop not registered")
            try
            {
                if (!Services.Has<EngineLoop>())
                {
                    // Try to instantiate via flexible helper first (will use container if possible)
                    if (!InstantiateAndRegister(typeof(EngineLoop)))
                    {
                        // Fallback: try to resolve SystemManager and create EngineLoop with it
                        try
                        {
                            SystemManager? sysMgr = null;
                            try { Services.TryResolve<SystemManager>(out sysMgr); } catch { sysMgr = null; }

                            object? instance = null;
                            if (sysMgr != null)
                            {
                                // Try constructor EngineLoop(SystemManager)
                                try { instance = Activator.CreateInstance(typeof(EngineLoop), sysMgr); } catch { instance = null; }
                            }

                            // Last resort: parameterless ctor (if exists)
                            if (instance == null)
                            {
                                try { instance = Activator.CreateInstance(typeof(EngineLoop)); } catch { instance = null; }
                            }

                            if (instance != null)
                            {
                                // Register both concrete type and any EngineLoop-like interface if present
                                try { Services.Register(typeof(EngineLoop), instance); } catch { /* ignore */ }

                                // If EngineLoop implements an interface like IEngineLoop, register that too
                                var iface = instance.GetType().GetInterfaces()
                                                .FirstOrDefault(i => i.Name.IndexOf("EngineLoop", StringComparison.OrdinalIgnoreCase) >= 0);
                                if (iface != null)
                                {
                                    try
                                    {
                                        var regGen = typeof(ServiceContainer).GetMethods()
                                            .FirstOrDefault(m => m.Name.Equals("Register", StringComparison.OrdinalIgnoreCase) && m.IsGenericMethod && m.GetParameters().Length == 1);
                                        if (regGen != null)
                                        {
                                            var constructed = regGen.MakeGenericMethod(iface);
                                            constructed.Invoke(Services, new object[] { instance });
                                        }
                                    }
                                    catch { /* ignore */ }
                                }

                                Logger.Info("EngineBootstrap: Registered default EngineLoop (post-ServiceLoader).");
                            }
                            else
                            {
                                Logger.Warn("EngineBootstrap: EngineLoop instance could not be created in fallback registration.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Warn($"EngineBootstrap: EngineLoop fallback registration failed: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Warn($"EngineBootstrap: EngineLoop presence check failed: {ex.Message}");
            }


            if (registeredCount == 0)
            {
                throw new Exception("No services registered by ServiceLoader. Ensure configuration lists services correctly.");
            }
        }


        private void RegisterBaseServices()
        {
            // 保证 ConfigManager 存在
            if (!Services.Has<ConfigManager>())
            {
                Services.Register(new ConfigManager());
            }

            // Core
            if (!Services.Has<EventBus>())
            {
                Services.Register(new EventBus());
            }

            if (!Services.Has<FactoryManager>())
            {
                Services.Register(new FactoryManager());
            }

            // SerializationManager
            if (!Services.Has<SerializationManager>())
            {
                if (!InstantiateAndRegister(typeof(SerializationManager)))
                {
                    Services.Register(new SerializationManager());
                }
            }

            // 确保默认 SaveData 序列化器存在（回退路径）
            try
            {
                if (Services.TryResolve<SerializationManager>(out var serMgr) && serMgr != null)
                {
                    if (!serMgr.TryGetSerializer<SaveData>(out var _))
                    {
                        serMgr.Register<SaveData>(new SaveDataJsonSerializer());
                        Logger.Info("EngineBootstrap: Registered default SaveDataJsonSerializer.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Warn($"EngineBootstrap: Could not register SaveData serializer: {ex.Message}");
            }

            // PersistenceManager
            if (!Services.Has<PersistenceManager>())
            {
                if (!InstantiateAndRegister(typeof(PersistenceManager)))
                {
                    if (Services.TryResolve<SerializationManager>(out var serMgr) && serMgr != null)
                    {
                        Services.Register(new PersistenceManager(serMgr));
                    }
                    else
                    {
                        try
                        {
                            var pm = Activator.CreateInstance<PersistenceManager>();
                            Services.Register(pm);
                        }
                        catch
                        {
                            Logger.Warn("EngineBootstrap: Could not create PersistenceManager via any known constructor.");
                        }
                    }
                }
            }

            // EntityFactory
            if (!Services.Has<EntityFactory>())
            {
                if (!InstantiateAndRegister(typeof(EntityFactory)))
                {
                    try
                    {
                        var entityFactory = new EntityFactory(Services);
                        Services.Register(entityFactory);
                    }
                    catch
                    {
                        Logger.Warn("EngineBootstrap: Failed to create EntityFactory via fallback.");
                    }
                }
            }


            // SystemManager
            if (!Services.Has<SystemManager>())
            {
                if (!InstantiateAndRegister(typeof(SystemManager)))
                {
                    try
                    {
                        var sysMgr = new SystemManager(Services);
                        Services.Register(sysMgr);
                    }
                    catch
                    {
                        Logger.Warn("EngineBootstrap: Failed to create SystemManager via fallback.");
                    }
                }
            }

            
            // RuntimeContext
            if (!Services.Has<RuntimeContext>())
            {
                if (!InstantiateAndRegister(typeof(RuntimeContext)))
                {
                    try
                    {
                        // Prefer DI ctor if possible
                        if (Services.TryResolve<ConfigManager>(out var cm) && cm != null)
                        {
                            var runtime = Activator.CreateInstance(typeof(RuntimeContext), Services, cm) as RuntimeContext;
                            if (runtime != null) Services.Register(runtime);
                            else
                            {
                                var runtime2 = Activator.CreateInstance<RuntimeContext>();
                                Services.Register(runtime2);
                            }
                        }
                        else
                        {
                            var runtime2 = Activator.CreateInstance<RuntimeContext>();
                            Services.Register(runtime2);
                        }
                    }
                    catch
                    {
                        Logger.Warn("EngineBootstrap: Failed to create RuntimeContext via fallback.");
                    }
                }
            }
        }

        private void RegisterModules()
        {
            // 尝试灵活构造 ModuleLoader（支持不同签名）
            if (!Services.TryResolve<ConfigManager>(out var cfg) || cfg == null)
            {
                Logger.Warn("EngineBootstrap: ConfigManager missing when registering modules.");
                return;
            }

            object? moduleLoaderInstance = null;
            var moduleLoaderType = typeof(ModuleLoader);

            // Try flexible ctor with (ServiceContainer, ConfigManager) or (ConfigManager, ServiceContainer) etc.
            moduleLoaderInstance = CreateInstanceWithFlexibleCtor(moduleLoaderType, Services, cfg)
                                   ?? CreateInstanceWithFlexibleCtor(moduleLoaderType, cfg, Services)
                                   ?? Activator.CreateInstance(moduleLoaderType);

            if (moduleLoaderInstance == null)
            {
                Logger.Warn("EngineBootstrap: Could not create ModuleLoader instance.");
                return;
            }

            // ModuleLoader.Load expects List<Genesis.Engine.Core.Config.ModuleDefinition>
            var modulesConfig = Configuration?.Modules ?? new List<Genesis.Engine.Core.Config.ModuleDefinition>();

            // Invoke Load via reflection
            var loadMethod = moduleLoaderType.GetMethod("Load", new Type[] { typeof(List<Genesis.Engine.Core.Config.ModuleDefinition>) });
            List<IEngineModule> modules;
            if (loadMethod != null)
            {
                modules = (List<IEngineModule>?)loadMethod.Invoke(moduleLoaderInstance, new object[] { modulesConfig }) ?? new List<IEngineModule>();
            }
            else
            {
                // fallback: try any Load method
                var anyLoad = moduleLoaderType.GetMethod("Load");
                if (anyLoad != null)
                {
                    var result = anyLoad.Invoke(moduleLoaderInstance, new object[] { modulesConfig });
                    modules = result as List<IEngineModule> ?? new List<IEngineModule>();
                }
                else
                {
                    Logger.Warn("EngineBootstrap: ModuleLoader.Load method not found.");
                    modules = new List<IEngineModule>();
                }
            }

            foreach (var module in modules)
            {
                Modules.Add(module);
            }
        }

        public void Start()
        {
            Logger.Info("Genesis Engine Starting");

            if (Services.TryResolve<RuntimeContext>(out var runtime) && runtime != null)
            {
                runtime.Start();
                Logger.Info("Genesis Engine Started");
            }
            else
            {
                Logger.Warn("Genesis Engine Start skipped: RuntimeContext not registered.");
            }
        }

        public void Update(float deltaTime)
        {
            if (Services.TryResolve<RuntimeContext>(out var runtime) && runtime != null)
            {
                runtime.Update(deltaTime);
            }
            else if (Services.TryResolve<SystemManager>(out var sysMgr) && sysMgr != null)
            {
                sysMgr.Update(deltaTime);
            }
        }

        public void Stop()
        {
            try
            {
                Modules.Shutdown();
            }
            catch (Exception ex)
            {
                Logger.Warn($"EngineBootstrap: Module shutdown error: {ex.Message}");
            }

            if (Services.TryResolve<SystemManager>(out var sysMgr) && sysMgr != null)
            {
                try { sysMgr.Shutdown(); }
                catch (Exception ex) { Logger.Warn($"SystemManager shutdown error: {ex.Message}"); }
            }

            if (Services.TryResolve<RuntimeContext>(out var runtime) && runtime != null)
            {
                try { runtime.Stop(); }
                catch (Exception ex) { Logger.Warn($"Runtime stop error: {ex.Message}"); }
            }
            else
            {
                Logger.Warn("RuntimeContext not registered at Stop.");
            }

            Logger.Info("Genesis Engine Stopped");
        }

        #region Helpers

        /// <summary>
        /// 更通用的构造器尝试器：接受两个可选对象（任意顺序），并智能识别 ServiceContainer / ConfigManager。
        /// 这样可以兼容不同构造器签名和调用顺序，避免参数类型不匹配的编译错误。
        /// </summary>
        private object? CreateInstanceWithFlexibleCtor(Type targetType, object? a = null, object? b = null)
        {
            // Identify candidates
            ServiceContainer? svc = null;
            ConfigManager? cfg = null;

            if (a is ServiceContainer s1) svc = s1;
            if (b is ServiceContainer s2) svc = svc ?? s2;

            if (a is ConfigManager c1) cfg = c1;
            if (b is ConfigManager c2) cfg = cfg ?? c2;

            // Try common ctor signatures in order
            ConstructorInfo? ctor;

            // (ConfigManager, ServiceContainer)
            if (cfg != null && svc != null)
            {
                ctor = targetType.GetConstructor(new Type[] { typeof(ConfigManager), typeof(ServiceContainer) });
                if (ctor != null)
                {
                    try { return ctor.Invoke(new object[] { cfg, svc }); }
                    catch { /* ignore and continue */ }
                }

                // (ServiceContainer, ConfigManager)
                ctor = targetType.GetConstructor(new Type[] { typeof(ServiceContainer), typeof(ConfigManager) });
                if (ctor != null)
                {
                    try { return ctor.Invoke(new object[] { svc, cfg }); }
                    catch { /* ignore and continue */ }
                }
            }

            // (ServiceContainer)
            if (svc != null)
            {
                ctor = targetType.GetConstructor(new Type[] { typeof(ServiceContainer) });
                if (ctor != null)
                {
                    try { return ctor.Invoke(new object[] { svc }); }
                    catch { /* ignore */ }
                }
            }

            // (ConfigManager)
            if (cfg != null)
            {
                ctor = targetType.GetConstructor(new Type[] { typeof(ConfigManager) });
                if (ctor != null)
                {
                    try { return ctor.Invoke(new object[] { cfg }); }
                    catch { /* ignore */ }
                }
            }

            // parameterless
            ctor = targetType.GetConstructor(Type.EmptyTypes);
            if (ctor != null)
            {
                try { return ctor.Invoke(null); }
                catch { /* ignore */ }
            }

            // 最后尝试任意 public ctor，尝试从容器满足参数
            var ctors = targetType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                                  .OrderByDescending(c => c.GetParameters().Length);
            foreach (var c in ctors)
            {
                var ps = c.GetParameters();
                var args = new object?[ps.Length];
                var ok = true;
                for (int i = 0; i < ps.Length; i++)
                {
                    var pType = ps[i].ParameterType;
                    if (pType == typeof(ServiceContainer) && svc != null) { args[i] = svc; continue; }
                    if (pType == typeof(ConfigManager) && cfg != null) { args[i] = cfg; continue; }
                    if (svc != null && svc.TryResolve(pType, out var resolved)) { args[i] = resolved; continue; }
                    ok = false; break;
                }
                if (!ok) continue;
                try { return c.Invoke(args); }
                catch { /* ignore and continue */ }
            }

            return null;
        }

        private void AutoRegisterSerializersFromContainer()
        {
            try
            {
                if (!Services.TryResolve<SerializationManager>(out var serMgr) || serMgr == null)
                {
                    Logger.Warn("EngineBootstrap: SerializationManager not found in container; skipping automatic serializer registration.");
                    return;
                }

                var allServices = Services.GetAll() ?? new List<ServiceDescriptor>();
                foreach (var sd in allServices)
                {
                    if (sd?.Instance == null) continue;
                    var inst = sd.Instance;
                    var interfaces = inst.GetType().GetInterfaces();
                    foreach (var iface in interfaces)
                    {
                        if (!iface.IsGenericType) continue;
                        if (iface.GetGenericTypeDefinition() != typeof(ISerializer<>)) continue;

                        var targetType = iface.GetGenericArguments().FirstOrDefault();
                        if (targetType == null) continue;

                        try
                        {
                            var registerMethodInfo = typeof(SerializationManager).GetMethod("Register");
                            if (registerMethodInfo == null) continue;

                            MethodInfo? registerMethod = null;
                            try
                            {
                                registerMethod = registerMethodInfo.MakeGenericMethod(targetType);
                            }
                            catch
                            {
                                continue;
                            }

                            registerMethod?.Invoke(serMgr, new object[] { inst });
                            Logger.Info($"EngineBootstrap: Registered serializer for {targetType.FullName} from service '{sd.ServiceType?.FullName ?? sd.GetType().FullName}'.");
                        }
                        catch (Exception ex)
                        {
                            Logger.Warn($"EngineBootstrap: Failed to register serializer for {targetType?.FullName ?? "<unknown>"}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Warn($"EngineBootstrap: Error while auto-registering serializers: {ex.Message}");
            }
        }


        private bool InstantiateAndRegister(Type targetType)
        {
            try
            {
                var cfg = Services.TryResolve<ConfigManager>(out var cm) ? cm : null;
                var instance = CreateInstanceWithFlexibleCtor(targetType, Services, cfg);
                if (instance == null) return false;

                try
                {
                    Services.Register(targetType, instance);
                    return true;
                }
                catch
                {
                    try
                    {
                        var regMethod = typeof(ServiceContainer).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                            .FirstOrDefault(m => m.Name == "Register" && m.IsGenericMethod && m.GetParameters().Length == 1);
                        if (regMethod != null)
                        {
                            var constructed = regMethod.MakeGenericMethod(targetType);
                            constructed.Invoke(Services, new object[] { instance });
                            return true;
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Logger.Warn($"EngineBootstrap: InstantiateAndRegister failed for {targetType.FullName}: {ex.Message}");
            }


            return false;
        }

        #endregion
    }
}
