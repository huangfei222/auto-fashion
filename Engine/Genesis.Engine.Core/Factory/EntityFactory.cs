using System;
using Genesis.Engine.Core.Config;
using Genesis.Engine.Core.Services;

namespace Genesis.Engine.Core.Factory
{
    /// <summary>
    /// EntityFactory - minimal, robust implementation that resolves ConfigManager from ServiceContainer safely.
    /// </summary>
    public class EntityFactory
    {
        private readonly ConfigManager configManager;

        public EntityFactory(ServiceContainer services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // Prefer generic TryResolve<T>
            if (services.TryResolve<ConfigManager>(out var cfg))
            {
                configManager = cfg!;
            }
            else
            {
                // Fallback: try non-generic resolution by Type
                if (services.TryResolve(typeof(ConfigManager), out var inst) && inst is ConfigManager cm)
                {
                    configManager = cm;
                }
                else
                {
                    // Last resort: throw a clear exception so caller can fix DI registration
                    throw new InvalidOperationException("EntityFactory: ConfigManager not registered in ServiceContainer.");
                }
            }
        }

        // Example factory method (adjust to your real entity creation API)
        public object Create(string entityType)
        {
            // Use configManager to create entity according to configuration
            // Placeholder: return a simple object or throw if unknown
            if (string.IsNullOrWhiteSpace(entityType)) throw new ArgumentNullException(nameof(entityType));
            // TODO: implement actual entity creation using configManager
            return new { Type = entityType };
        }
    }
}
