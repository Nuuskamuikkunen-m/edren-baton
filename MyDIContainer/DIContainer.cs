using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDIContainer
{
    class DIContainer
    {
        private List<ServiceDescriptor> dependencies;
        public DIContainer()
        {
            dependencies = new List<ServiceDescriptor>();
        }
        public void AddTransient<TService, TImplementation>()
        {
            dependencies.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceImplementation.Transient));
        }
        public void AddSingleton<TService, TImplementation>()
        {
            dependencies.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceImplementation.Singleton));
        }
        public T Get<T>() => (T)Get(typeof(T));
        public object Get(Type serviceType)
        {
            var descriptor = dependencies.SingleOrDefault(x => x.ServiceType == serviceType);
            if (descriptor == null)
            {
                throw new Exception("cервіс не знайдено");
            }
            if (descriptor.Implementation != null)
            {
                return descriptor.Implementation;
            }
            var actualType = descriptor.ImplementationType;
            var constructor = actualType.GetConstructors().First();
            if (constructor.GetParameters().Any(x => IsItCycled(serviceType, x.ParameterType)))
            {
                throw new Exception("цiкл");
            }
            var parameters = constructor.GetParameters().Select(x => Get(x.ParameterType)).ToArray();
            var implementation = Activator.CreateInstance(actualType, parameters);
            if (descriptor.LifeTime == ServiceImplementation.Singleton)
            {
                descriptor.Implementation = implementation;
            }
            return implementation;
        }
        public bool IsItCycled(Type serviceType, Type parametrType)
        {
            var descriptor = dependencies.SingleOrDefault(x => x.ServiceType == parametrType);
            var actualType = descriptor.ImplementationType;
            var constructorType = actualType.GetConstructors().First();
            return constructorType.GetParameters().Any(x => Equals(serviceType, x.ParameterType));
        }
    }
}
