using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Genesis.Engine.Core.Services
{
    /// <summary>
    /// 简单的服务容器
    /// - 支持 Register(Type, object), Register<T>(T)
    /// - 支持 TryResolve<T>(out T), Get<T>(), ResolveAssignable(Type)
    /// - 支持 GetAll() 返回 ServiceDescriptor 列表（供引导/扫描使用）
    /// </summary>
    public class ServiceContainer
    {
        private readonly ConcurrentDictionary<Type, object> instances = new();

        public void Register(Type type, object instance)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            instances[type] = instance;
        }

        public void Register<T>(T instance)
        {
            Register(typeof(T), instance!);
        }

        public bool Has<T>()
        {
            return instances.ContainsKey(typeof(T));
        }

        public bool TryResolve<T>(out T? instance)
        {
            if (instances.TryGetValue(typeof(T), out var obj) && obj is T t)
            {
                instance = t;
                return true;
            }
            instance = default;
            return false;
        }

        public bool TryResolve(Type type, out object? instance)
        {
            if (instances.TryGetValue(type, out var obj))
            {
                instance = obj;
                return true;
            }
            instance = null;
            return false;
        }

        public T Get<T>()
        {
            if (TryResolve<T>(out var inst) && inst != null) return inst;
            throw new InvalidOperationException($"ServiceContainer: Service of type {typeof(T).FullName} not registered.");
        }

        public object Get(Type type)
        {
            if (TryResolve(type, out var inst) && inst != null) return inst;
            throw new InvalidOperationException($"ServiceContainer: Service of type {type.FullName} not registered.");
        }

        public object? ResolveAssignable(Type targetType)
        {
            // 返回第一个可赋值给 targetType 的实例
            foreach (var kv in instances)
            {
                if (targetType.IsAssignableFrom(kv.Key))
                {
                    return kv.Value;
                }
            }
            return null;
        }

        public IEnumerable<ServiceDescriptor> GetAll()
        {
            return instances.Select(kv => new ServiceDescriptor(kv.Key, kv.Value)).ToList();
        }
    }
}
