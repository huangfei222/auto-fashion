using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Genesis.Engine.Core.Config;
using Genesis.Engine.Core.Logging;
using Genesis.Engine.Core.Runtime.Serialization;

namespace Genesis.Engine.Core.Services
{
    /// <summary>
    /// ServiceLoader
    /// - 从配置加载服务定义并按 DependsOn 拓扑排序
    /// - 通过反射实例化服务（支持构造器注入 ConfigManager / ServiceContainer / 已注册服务 / JsonElement config）
    /// - 将实例注册到 ServiceContainer（兼容多种 Register 签名）
    /// - 自动把实现 ISerializer<T> 的实例注册到 SerializationManager（如果存在）
    /// </summary>
    public class ServiceLoader
    {
        private readonly ConfigManager configManager;
        private readonly ServiceContainer serviceContainer;
        private readonly Type serviceContainerType;

        public ServiceLoader(ConfigManager configManager, ServiceContainer serviceContainer)
        {
            this.configManager = configManager ?? throw new ArgumentNullException(nameof(configManager));
            this.serviceContainer = serviceContainer ?? throw new ArgumentNullException(nameof(serviceContainer));
            this.serviceContainerType = serviceContainer.GetType();
        }

        public void LoadServices()
        {
            EngineConfiguration cfg;
            try
            {
                cfg = configManager.Get<EngineConfiguration>();
            }
            catch (Exception ex)
            {
                LogError($"ServiceLoader: EngineConfiguration not found in ConfigManager. {ex.Message}");
                throw;
            }

            var defs = cfg?.Services?.Services ?? new List<ServiceDefinition>();
            LogInfo($"ServiceLoader: Found {defs.Count} service definitions.");

            var enabledDefs = defs.Where(d => d != null && d.Enabled).ToList();

            var idMap = new Dictionary<string, ServiceDefinition>(StringComparer.OrdinalIgnoreCase);
            foreach (var d in enabledDefs)
            {
                if (d == null) continue;
                var id = string.IsNullOrWhiteSpace(d.Id) ? d.Type : d.Id;
                if (string.IsNullOrWhiteSpace(id)) id = d.Type ?? "<unknown>";
                if (!idMap.ContainsKey(id))
                {
                    idMap[id] = d;
                }
                else
                {
                    LogWarning($"ServiceLoader: Duplicate service id '{id}' found. Later entry ignored.");
                }
            }

            var sorted = TopologicalSort(enabledDefs, idMap);

            if (sorted == null)
            {
                LogError("ServiceLoader: Circular dependency detected among services. Aborting service load.");
                return;
            }

            foreach (var def in sorted)
            {
                if (def == null)
                {
                    LogWarning("ServiceLoader: Encountered null service definition; skipping.");
                    continue;
                }

                try
                {
                    var typeName = def.Type?.Trim();
                    if (string.IsNullOrWhiteSpace(typeName))
                    {
                        LogError("ServiceLoader: Service definition missing Type; skipping.");
                        continue;
                    }

                    var svcType = ResolveType(typeName, def.Assembly);
                    if (svcType == null)
                    {
                        LogError($"ServiceLoader: Cannot resolve type '{typeName}'. Skipping service '{def.Id ?? def.Type}'.");
                        continue;
                    }

                    if (svcType.IsAbstract || (svcType.IsSealed && svcType.IsAbstract))
                    {
                        LogWarning($"ServiceLoader: Service type '{svcType.FullName}' is static or abstract and cannot be instantiated. Skipping registration.");
                        continue;
                    }

                    var instance = CreateInstance(svcType, def);
                    if (instance == null)
                    {
                        LogError($"ServiceLoader: Failed to instantiate service type '{svcType.FullName}'.");
                        continue;
                    }

                    var registered = TryRegisterInstance(svcType, instance);
                    if (registered)
                    {
                        LogInfo($"ServiceLoader: Registered service '{svcType.FullName}' (id='{def.Id}').");
                        TryAutoRegisterSerializers(instance);
                    }
                    else
                    {
                        LogWarning($"ServiceLoader: Could not register service '{svcType.FullName}' - no compatible Register method found.");
                    }
                }
                catch (Exception ex)
                {
                    LogError($"ServiceLoader: Exception while loading service '{def?.Type}': {ex.Message}");
                }
            }
        }

        private List<ServiceDefinition>? TopologicalSort(List<ServiceDefinition> defs, Dictionary<string, ServiceDefinition> idMap)
        {
            var inDegree = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var graph = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            foreach (var d in defs)
            {
                var id = string.IsNullOrWhiteSpace(d?.Id) ? d?.Type : d?.Id;
                if (string.IsNullOrWhiteSpace(id)) id = d?.Type ?? "<unknown>";
                if (!inDegree.ContainsKey(id)) inDegree[id] = 0;
                if (!graph.ContainsKey(id)) graph[id] = new List<string>();
            }

            foreach (var d in defs)
            {
                if (d == null) continue;
                var id = string.IsNullOrWhiteSpace(d.Id) ? d.Type : d.Id;
                if (string.IsNullOrWhiteSpace(id)) id = d.Type ?? "<unknown>";

                foreach (var dep in d.DependsOn ?? Enumerable.Empty<string>())
                {
                    if (string.IsNullOrWhiteSpace(dep)) continue;
                    var depId = dep.Trim();
                    if (!idMap.ContainsKey(depId))
                    {
                        LogWarning($"ServiceLoader: Service '{id}' depends on unknown service '{depId}'. Ignoring missing dependency.");
                        continue;
                    }

                    if (!graph.ContainsKey(depId)) graph[depId] = new List<string>();
                    graph[depId].Add(id);
                    inDegree[id] = inDegree.ContainsKey(id) ? inDegree[id] + 1 : 1;
                }
            }

            var queue = new Queue<string>(inDegree.Where(kv => kv.Value == 0).Select(kv => kv.Key));
            var resultIds = new List<string>();

            while (queue.Count > 0)
            {
                var n = queue.Dequeue();
                resultIds.Add(n);
                if (!graph.ContainsKey(n)) continue;
                foreach (var m in graph[n])
                {
                    inDegree[m]--;
                    if (inDegree[m] == 0) queue.Enqueue(m);
                }
            }

            if (resultIds.Count != inDegree.Count)
            {
                return null;
            }

            var idToDef = new Dictionary<string, ServiceDefinition>(StringComparer.OrdinalIgnoreCase);
            foreach (var d in defs)
            {
                if (d == null) continue;
                var id = string.IsNullOrWhiteSpace(d.Id) ? d.Type : d.Id;
                if (string.IsNullOrWhiteSpace(id)) id = d.Type ?? "<unknown>";
                idToDef[id] = d;
            }

            var sorted = resultIds.Select(id => idToDef[id]).ToList();
            return sorted;
        }

        private Type? ResolveType(string typeName, string? assemblyName)
        {
            if (string.IsNullOrWhiteSpace(typeName)) return null;

            var t = Type.GetType(typeName, false, true);
            if (t != null) return t;

            if (!string.IsNullOrWhiteSpace(assemblyName))
            {
                try
                {
                    var asm = AppDomain.CurrentDomain.GetAssemblies()
                        .FirstOrDefault(a => string.Equals(a.GetName().Name, assemblyName, StringComparison.OrdinalIgnoreCase));

                    if (asm == null)
                    {
                        try { asm = Assembly.Load(new AssemblyName(assemblyName)); } catch { asm = null; }
                    }

                    if (asm != null)
                    {
                        var found = asm.GetTypes().FirstOrDefault(x =>
                            string.Equals(x.FullName, typeName, StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(x.Name, typeName, StringComparison.OrdinalIgnoreCase));
                        if (found != null) return found;
                    }
                }
                catch (Exception ex)
                {
                    LogWarning($"ServiceLoader: Assembly load/search failed for '{assemblyName}': {ex.Message}");
                }
            }

            var loaded = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in loaded)
            {
                try
                {
                    var found = asm.GetTypes().FirstOrDefault(x =>
                        string.Equals(x.FullName, typeName, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(x.Name, typeName, StringComparison.OrdinalIgnoreCase));
                    if (found != null) return found;
                }
                catch (ReflectionTypeLoadException) { }
            }

            return null;
        }

        private object? CreateInstance(Type svcType, ServiceDefinition def)
        {
            if (svcType.IsAbstract || (svcType.IsSealed && svcType.IsAbstract))
            {
                LogWarning($"ServiceLoader: Type {svcType.FullName} is static/abstract and cannot be instantiated.");
                return null;
            }

            var ctor0 = svcType.GetConstructor(Type.EmptyTypes);
            if (ctor0 != null)
            {
                try { return Activator.CreateInstance(svcType); }
                catch (Exception ex) { LogWarning($"ServiceLoader: Parameterless ctor failed for {svcType.FullName}: {ex.Message}"); }
            }

            var ctors = svcType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                               .OrderByDescending(c => c.GetParameters().Length);

            foreach (var ctor in ctors)
            {
                var parameters = ctor.GetParameters();
                var args = new object?[parameters.Length];
                var canUse = true;

                for (int i = 0; i < parameters.Length; i++)
                {
                    var pType = parameters[i].ParameterType;

                    if (pType == typeof(ConfigManager) || pType.IsAssignableFrom(typeof(ConfigManager)))
                    {
                        args[i] = configManager;
                        continue;
                    }

                    if (pType == typeof(ServiceContainer) || pType.IsAssignableFrom(typeof(ServiceContainer)))
                    {
                        args[i] = serviceContainer;
                        continue;
                    }

                    if (pType == typeof(JsonElement))
                    {
                        // def may be null or def.Config may be default; assign safely
                        args[i] = def?.Config ?? default(JsonElement);
                        continue;
                    }

                    if (TryResolveFromContainer(pType, out var resolved))
                    {
                        args[i] = resolved;
                        continue;
                    }

                    canUse = false;
                    break;
                }

                if (!canUse) continue;

                try { return ctor.Invoke(args); }
                catch (Exception ex) { LogWarning($"ServiceLoader: Constructor invocation failed for {svcType.FullName}: {ex.Message}"); }
            }

            return null;
        }

        private bool TryResolveFromContainer(Type targetType, out object? instance)
        {
            instance = null;
            try
            {
                var tryResolveGeneric = serviceContainerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(m => m.Name.Equals("TryResolve", StringComparison.OrdinalIgnoreCase) && m.IsGenericMethod && m.GetParameters().Length == 1);

                if (tryResolveGeneric != null)
                {
                    var constructed = tryResolveGeneric.MakeGenericMethod(targetType);
                    var parameters = new object[] { null! };
                    var ok = (bool)constructed.Invoke(serviceContainer, parameters)!;
                    if (ok)
                    {
                        instance = parameters[0];
                        return true;
                    }
                }

                var getByType = serviceContainerType.GetMethod("Get", new Type[] { typeof(Type) });
                if (getByType != null)
                {
                    try
                    {
                        var inst = getByType.Invoke(serviceContainer, new object[] { targetType });
                        if (inst != null) { instance = inst; return true; }
                    }
                    catch { }
                }

                var resolveAssignable = serviceContainerType.GetMethod("ResolveAssignable", new Type[] { typeof(Type) });
                if (resolveAssignable != null)
                {
                    var inst = resolveAssignable.Invoke(serviceContainer, new object[] { targetType });
                    if (inst != null) { instance = inst; return true; }
                }
            }
            catch { }

            return false;
        }

        private bool TryRegisterInstance(Type svcType, object instance)
        {
            try
            {
                var regTypeObj = serviceContainerType.GetMethod("Register", new Type[] { typeof(Type), typeof(object) });
                if (regTypeObj != null)
                {
                    regTypeObj.Invoke(serviceContainer, new object[] { svcType, instance });
                    return true;
                }

                var regGeneric = serviceContainerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(m => m.Name.Equals("Register", StringComparison.OrdinalIgnoreCase) && m.IsGenericMethod && m.GetParameters().Length == 1);
                if (regGeneric != null)
                {
                    var constructed = regGeneric.MakeGenericMethod(svcType);
                    constructed.Invoke(serviceContainer, new object[] { instance });
                    return true;
                }

                var sdCtor = typeof(ServiceDescriptor).GetConstructor(new Type[] { typeof(Type), typeof(object) });
                if (sdCtor != null)
                {
                    var descriptor = sdCtor.Invoke(new object[] { svcType, instance });
                    var regSd = serviceContainerType.GetMethod("Register", new Type[] { typeof(ServiceDescriptor) });
                    if (regSd != null)
                    {
                        regSd.Invoke(serviceContainer, new object[] { descriptor });
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogWarning($"ServiceLoader: Register attempt threw: {ex.Message}");
            }

            return false;
        }

#pragma warning disable CS8601
        private void TryAutoRegisterSerializers(object instance)
        {
            string instTypeName = instance?.GetType().FullName ?? "<unknown>";

            try
            {
                if (!serviceContainer.TryResolve<SerializationManager>(out var serMgr) || serMgr == null)
                {
                    LogInfo($"ServiceLoader: SerializationManager not found; skipping auto-register for instance {instTypeName}.");
                    return;
                }

                var instType = instance?.GetType();
                if (instType == null)
                {
                    LogInfo($"ServiceLoader: Instance type is null; skipping auto-register for instance {instTypeName}.");
                    return;
                }

                var interfaces = instType.GetInterfaces() ?? Array.Empty<Type>();

                foreach (var iface in interfaces)
                {
                    if (iface == null) continue;
                    if (!iface.IsGenericType) continue;

                    Type? genDef;
                    try { genDef = iface.GetGenericTypeDefinition(); }
                    catch { continue; }

                    if (genDef != typeof(ISerializer<>)) continue;

                    var genericArgs = iface.GetGenericArguments();
                    if (genericArgs == null || genericArgs.Length == 0) continue;

                    var targetType = genericArgs[0];
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

                        if (registerMethod == null) continue;

                        registerMethod.Invoke(serMgr, new object[] { instance });

                        var targetName = targetType.FullName ?? targetType.Name ?? "<unknown>";
                        var instName = instType.FullName ?? instTypeName;
                        LogInfo($"ServiceLoader: Auto-registered serializer for {targetName} from instance {instName}.");
                    }
                    catch (TargetInvocationException tie)
                    {
                        var tn = targetType?.FullName ?? "<unknown>";
                        LogWarning($"ServiceLoader: Failed to auto-register serializer for {tn}: {tie.InnerException?.Message ?? tie.Message}");
                    }
                    catch (Exception ex)
                    {
                        var tn = targetType?.FullName ?? "<unknown>";
                        LogWarning($"ServiceLoader: Failed to auto-register serializer for {tn}: {ex.Message}");
                    }
                }

                LogInfo($"ServiceLoader: Auto-register serializers completed for instance {instTypeName}.");
            }
            catch (Exception ex)
            {
                LogWarning($"ServiceLoader: Error during auto-registering serializers for {instTypeName}: {ex.Message}");
            }
        }


#pragma warning restore CS8601

        private void LogInfo(string msg) => TryLoggerInvoke("Info", msg);
        private void LogWarning(string msg) => TryLoggerInvoke("Warn", msg);
        private void LogError(string msg) => TryLoggerInvoke("Error", msg);

        private void TryLoggerInvoke(string levelMethod, string msg)
        {
            try
            {
                var loggerType = Type.GetType("Genesis.Engine.Core.Logging.Logger, Genesis.Engine.Core");
                if (loggerType != null)
                {
                    var method = loggerType.GetMethod(levelMethod, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
                    method?.Invoke(null, new object[] { msg });
                    return;
                }
            }
            catch { }

            Console.WriteLine($"[{levelMethod}] {msg}");
        }
    }
}
