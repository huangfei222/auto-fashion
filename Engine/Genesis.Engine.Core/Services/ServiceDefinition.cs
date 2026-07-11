using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Genesis.Engine.Core.Services
{
    public class ServiceDefinition
    {
        public string Type { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Assembly { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
        public List<string> DependsOn { get; set; } = new();
        public JsonElement Config { get; set; }

        public ServiceDefinition()
        {
            Type = string.Empty;
            Id = string.Empty;
            Assembly = string.Empty;
            Enabled = true;
            DependsOn = new List<string>();
            Config = JsonDocument.Parse("{}").RootElement;
        }
    }
}
