namespace IoCComparison.AutoregisterTests.UnityExtensions
{
    using Unity.AutoRegistration;

    public static class UnityRegistrationExtensions
    {
        /// <summary>
        /// Register a type as itself (as opposed to its interfaces)
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        public static IFluentRegistration AsSelf(this IFluentRegistration registration)
        {
            return registration.As(t => t);
        }
    }
}
