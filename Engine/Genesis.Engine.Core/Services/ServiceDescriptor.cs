using System;

namespace Genesis.Engine.Core.Services
{
    /// <summary>
    /// ServiceDescriptor 用于在 ServiceContainer 中描述已注册的服务实例或工厂
    /// </summary>
    public class ServiceDescriptor
    {
        public Type ServiceType { get; }
        public object Instance { get; }

        public ServiceDescriptor(Type serviceType, object instance)
        {
            ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }
    }
}
