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

        public object Get(Type serviceType)
        {
            List<Type> listik = new List<Type>();

            return Get(serviceType, listik);
        }
        //public T Get<T>() => (T)Get(typeof(T));

        public object Get(Type serviceType, List<Type> parlist)
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
            List<object> sobakaobaka = new List<object>();

            foreach (var parameter in constructor.GetParameters())
            {
                if (parlist.Contains(serviceType))
                {
                    throw new Exception("цiкл");
                }
                parlist.Add(serviceType);

                var newParameter = Get(parameter.ParameterType, parlist);

                parlist.Remove(serviceType);

                sobakaobaka.Add(newParameter);
            }


            var parameters = sobakaobaka.ToArray();
            var implementation = Activator.CreateInstance(actualType, parameters);
            if (descriptor.LifeTime == ServiceImplementation.Singleton)
            {
                descriptor.Implementation = implementation;
            }
            return implementation;
        }

      


        public bool IsItCycled(Type serviceType, ref List<object> parlist) //oioio
        {
            if (parlist.Contains(serviceType))
            {
                return true;
            }

            return false;           
           
        }

       
    }
}
