using System;
using Spring.Context.Support;
using Spring.Objects.Factory.Config;
using Spring.Objects.Factory.Support;

namespace IoCComparison
{
    public static class SpringHelper
    {
        public static void RegisterType<T>(this GenericApplicationContext context, string name)
        {
            context.RegisterType(name, typeof(T));
        }

        public static void RegisterSingleton<T>(this GenericApplicationContext context, string name)
        {
            context.RegisterSingleton(name, typeof(T));
        }

        public static void RegisterType(this GenericApplicationContext context, string name, Type type)
        {
            IObjectDefinitionFactory objectDefinitionFactory = new DefaultObjectDefinitionFactory();
            ObjectDefinitionBuilder builder = ObjectDefinitionBuilder.RootObjectDefinition(objectDefinitionFactory, type);
            builder.SetAutowireMode(AutoWiringMode.AutoDetect);
            builder.SetSingleton(false);

            context.RegisterObjectDefinition(name, builder.ObjectDefinition);
        }

        public static void RegisterSingleton(this GenericApplicationContext context, string name, Type type)
        {
            IObjectDefinitionFactory objectDefinitionFactory = new DefaultObjectDefinitionFactory();
            ObjectDefinitionBuilder builder = ObjectDefinitionBuilder.RootObjectDefinition(objectDefinitionFactory, type);
            builder.SetAutowireMode(AutoWiringMode.AutoDetect);
            builder.SetSingleton(true);

            context.RegisterObjectDefinition(name, builder.ObjectDefinition);
        }
    }
}
