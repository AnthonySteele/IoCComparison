namespace IoCComparison.AutoregisterTests.TypeExtensions
{
    using System;

    public static class TypeHelpers
    {
        public static bool HasInterfaces(this Type type)
        {
            return type.GetInterfaces().Length > 0;
        }

        public static bool IsSystemType(this Type type)
        {
            // Check the namespace prefix. Is there a better way to test for system types?
            return type.FullName.StartsWith("System.");
        }
    }
}
