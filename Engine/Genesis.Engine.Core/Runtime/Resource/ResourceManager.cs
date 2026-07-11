using System;
using System.Collections.Concurrent;
using Genesis.Engine.Core.Logging;

namespace Genesis.Engine.Core.Runtime.Resource
{
    public class ResourceManager
    {
        private readonly ConcurrentDictionary<string, object> resources = new();

        public void Register<T>(T resource) where T : IResource
        {
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            resources[resource.Key] = resource;
            Logger.Info($"ResourceManager: Registered resource {resource.Key}");
        }

        public bool TryGet<T>(string key, out T? resource) where T : class, IResource
        {
            if (resources.TryGetValue(key, out var obj) && obj is T r)
            {
                resource = r;
                return true;
            }

            resource = null;
            return false;
        }

        public T Get<T>(string key) where T : class, IResource
        {
            if (TryGet<T>(key, out var r) && r != null) return r;
            throw new KeyNotFoundException($"ResourceManager: Resource not found {key}");
        }

        public bool Remove(string key)
        {
            return resources.TryRemove(key, out _);
        }
    }
}
