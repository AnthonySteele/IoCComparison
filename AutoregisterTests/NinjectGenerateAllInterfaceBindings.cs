namespace IoCComparison.AutoregisterTests
{
    using System;
    using Ninject;
    using Ninject.Activation;
    using Ninject.Extensions.Conventions;

    /// <summary>
    /// generator to bind classes to all of thier interfaces
    /// even if the name differs
    /// </summary>
    public class NinjectGenerateAllInterfaceBindings : IBindingGenerator
    {
        public void Process(Type type, Func<IContext, object> scopeCallback, IKernel kernel)
        {
            if (type.IsInterface || type.IsAbstract)
            {
                return;
            }

            Type[] interfaceTypes = type.GetInterfaces();

            foreach (Type interfaceType in interfaceTypes)
            {
                kernel.Bind(interfaceType).To(type).InScope(scopeCallback);
            }
        }
    }
}
