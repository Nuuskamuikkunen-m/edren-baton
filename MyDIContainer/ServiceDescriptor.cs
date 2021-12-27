using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDIContainer
{
    class ServiceDescriptor
    {
        public Type ServiceType { get; }

        public Type ImplementationType { get; }

        public object Implementation { get; internal set; }

        public ServiceImplementation LifeTime { get; }

        public ServiceDescriptor(Type serviceType, Type implementationType, ServiceImplementation lifeTime)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
            LifeTime = lifeTime;
        }
    }
}
