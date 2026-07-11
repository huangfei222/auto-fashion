using System.Collections.Generic;
using System.Text.Json;
using Genesis.Engine.Core.Services;

namespace Genesis.Engine.Core.Config
{
    public class EngineConfiguration
    {
        public List<ModuleDefinition> Modules { get; set; } = new();
        public ServicesSection? Services { get; set; }
        public JsonElement Runtime { get; set; }

        public class ServicesSection
        {
            public List<ServiceDefinition> Services { get; set; } = new();
        }
    }

    public class ModuleDefinition
    {
        public string Type { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
        public int Order { get; set; } = 0;
        public JsonElement Config { get; set; }
    }
}
