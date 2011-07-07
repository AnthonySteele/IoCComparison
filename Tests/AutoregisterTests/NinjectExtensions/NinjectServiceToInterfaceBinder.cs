namespace IoCComparison.AutoregisterTests.NinjectExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ninject;
    using Ninject.Activation;
    using Ninject.Extensions.Conventions;
    using IoCComparison.AutoregisterTests.TypeExtensions;

    /// <summary>
    /// I think that the convention needed for registering services is
    /// 
    /// - Bind concrete classes (and open generics) to thier interfaces
    ///  -- except when that interface is in the System namespace. 
    ///   i.e. if FooService implements IDisposable or IList&ltT&gt, we don't want to bind it to that
    /// 
    /// - There is no need to test that interface names map to class names. 
    ///   The usual case is that FooService implements IFooService, but that falls down where there are multiple implementations,
    ///   e.g. the interface IValidator is implemented by NameValidator, OrderValidator etc.
    ///   The "no system interfaces" rule is closer to what's actually desired.
    /// 
    ///  - This code is adapted from Ninject.Extensions.Conventions.RegexBindingGenerator 
    ///  at https://github.com/ninject/ninject.extensions.conventions/blob/master/src/Ninject.Extensions.Conventions/RegexBindingGenerator.cs
    /// </summary>
    public class NinjectServiceToInterfaceBinder : IBindingGenerator
    {
        public void Process(Type type, Func<IContext, object> scopeCallback, IKernel kernel)
        {
            if (type.IsInterface || type.IsAbstract)
            {
                return;
            }

            IList<Type> interfaceTypes = type.GetInterfaces()
                .Where(t => ! t.IsSystemType()).ToList();

            if (interfaceTypes.Count > 0)
            {
                foreach (Type interfaceType in interfaceTypes)
                {
                    kernel.Bind(interfaceType).To(type).InScope(scopeCallback);
                }
            }
            else
            {
                // if the type has no interfaces - bind to self
                kernel.Bind(type).To(type).InScope(scopeCallback);
            }
        }
    }
}