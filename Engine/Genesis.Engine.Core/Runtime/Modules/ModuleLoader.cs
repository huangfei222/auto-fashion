using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Genesis.Engine.Core.Config;
using Genesis.Engine.Core.Logging;
using Genesis.Engine.Core.Services;

namespace Genesis.Engine.Core.Runtime.Modules
{
    public class ModuleLoader
    {
        private readonly ServiceContainer services;
        private readonly Genesis.Engine.Core.Config.ConfigManager configManager;

        public ModuleLoader(ServiceContainer services, Genesis.Engine.Core.Config.ConfigManager configManager)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.configManager = configManager ?? throw new ArgumentNullException(nameof(configManager));
        }

        public List<IEngineModule> Load(List<Genesis.Engine.Core.Config.ModuleDefinition> defs)
        {
            var modules = new List<IEngineModule>();
            if (defs == null || defs.Count == 0) return modules;

            var enabled = defs.Where(d => d != null && d.Enabled).OrderBy(d => d.Order).ToList();

            foreach (var def in enabled)
            {
                try
                {
                    var typeName = def.Type?.Trim();
                    if (string.IsNullOrWhiteSpace(typeName)) continue;

                    var t = Type.GetType(typeName, false, true);
                    if (t == null)
                    {
                        // try search loaded assemblies
                        t = AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(a => a.GetTypesSafe())
                            .FirstOrDefault(x => string.Equals(x.FullName, typeName, StringComparison.OrdinalIgnoreCase) || string.Equals(x.Name, typeName, StringComparison.OrdinalIgnoreCase));
                    }

                    if (t == null)
                    {
                        Logger.Warn($"ModuleLoader: Could not resolve module type '{typeName}'. Skipping.");
                        continue;
                    }

                    if (!typeof(IEngineModule).IsAssignableFrom(t))
                    {
                        Logger.Warn($"ModuleLoader: Type '{t.FullName}' does not implement IEngineModule. Skipping.");
                        continue;
                    }

                    var module = (IEngineModule?)Activator.CreateInstance(t);
                    if (module == null)
                    {
                        Logger.Warn($"ModuleLoader: Failed to instantiate module '{t.FullName}'.");
                        continue;
                    }

                    modules.Add(module);
                }
                catch (Exception ex)
                {
                    Logger.Warn($"ModuleLoader: Exception loading module '{def.Type}': {ex.Message}");
                }
            }

            return modules;
        }
    }

    static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetTypesSafe(this Assembly asm)
        {
            try { return asm.GetTypes(); }
            catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t != null)!; }
            catch { return Enumerable.Empty<Type>(); }
        }
    }
}
