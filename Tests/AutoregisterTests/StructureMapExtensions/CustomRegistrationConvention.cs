namespace IoCComparison.AutoregisterTests.StructureMapExtensions
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using StructureMap.Graph;
    using StructureMap.Configuration.DSL;
    using StructureMap.TypeRules;
    using IoCComparison.AutoregisterTests.TypeExtensions;

    public class CustomRegistrationConvention : IRegistrationConvention
    {
        private readonly List<Type> singletonTypes = new List<Type>();

        public CustomRegistrationConvention WithSingleton<T>()
        {
            singletonTypes.Add(typeof(T));
            return this;
        }

        public void Process(Type type, Registry registry)
        {
            if (!type.IsConcrete() || !type.CanBeCreated())
            {
                return;
            }

            IList<Type> interfaceTypes = type.GetInterfaces()
                .Where(t => !t.IsSystemType()).ToList();

            if (interfaceTypes.Count > 0)
            {
                foreach (Type interfaceType in interfaceTypes)
                {
                    Register(registry, interfaceType, type);
                }
            }
            else
            {
                // if the type has no interfaces - bind to self
                Register(registry, type, type);
            }
        }

        private void Register(Registry registry, Type sourceType, Type targetType)
        {
            if (ShouldRegisterAsSingleton(sourceType))
            {
                registry.For(sourceType).Singleton().Use(targetType);
            }
            else
            {
                registry.For(sourceType).Use(targetType);
            }
        }

        private bool ShouldRegisterAsSingleton(Type type)
        {
            return singletonTypes.Contains(type);
        }
    }
}
