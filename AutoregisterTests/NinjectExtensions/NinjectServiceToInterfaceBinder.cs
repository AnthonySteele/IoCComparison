using System;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.Conventions;

namespace IoCComparison.AutoregisterTests.NinjectExtensions
{
    /// <summary>
    /// I think that the convention needed for registering services is
    /// - Bind concrete classes (and open generics) to thier interfaces
    ///  -- except when that interface is in the system namespace. 
    ///   i.e. if FooService implements IDisposable or IList&ltT&gt, we don't want to bind it to that
    /// - Allow the consumer to specify a where lambda to filter them. 
    ///   -- Include filters are easier to understand than exclude filters - no need to invert the test
    /// </summary>
    public class NinjectServiceToInterfaceBinder : IBindingGenerator
    {
        private readonly Func<Type, bool> whereFilter;

        public NinjectServiceToInterfaceBinder()
        {
            whereFilter = t => true;
        }

        public NinjectServiceToInterfaceBinder(Func<Type, bool> whereFilter)
        {
            this.whereFilter = whereFilter;
        }

        public void Process(Type type, Func<IContext, object> scopeCallback, IKernel kernel)
        {
            if (type.IsInterface || type.IsAbstract)
            {
                return;
            }

            Type[] interfaceTypes = type.GetInterfaces();

            foreach (Type interfaceType in interfaceTypes)
            {
                if (! IsSystemType(interfaceType) && whereFilter(interfaceType))
                {
                    kernel.Bind(interfaceType).To(type).InScope(scopeCallback);
                }
            }
        }

        private static bool IsSystemType(Type type)
        {
            // Check the namespace prefix. Is there a better way to test for system types?
            return type.FullName.StartsWith("System.");
        }
    }
}