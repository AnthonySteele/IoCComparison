﻿namespace IoCComparison.AutoregisterTests.StructureMapExtensions
{
    using System;
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

            Type[] interfaceTypes = type.GetInterfaces();

            if (interfaceTypes.Length != 0)
            {
                foreach (Type interfaceType in interfaceTypes)
                {
                    if (! interfaceType.IsSystemType())
                    {
                        Register(registry, interfaceType, type);
                    }
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
            if (RegisterAsSingleton(sourceType))
            {
                registry.For(sourceType).Singleton().Use(targetType);
            }
            else
            {
                registry.For(sourceType).Use(targetType);
            }
        }

        private bool RegisterAsSingleton(Type type)
        {
            return singletonTypes.Contains(type);
        }
    }
}