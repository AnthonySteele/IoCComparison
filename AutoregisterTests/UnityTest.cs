using System.Reflection;

namespace IoCComparison.AutoregisterTests
{
    using System;
    using System.Linq;
    using AutoregisteredClasses.Interfaces;
    using AutoregisteredClasses.Services;
    using Unity.AutoRegistration;
    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    // need the "Unity Auto Registration" addon
    // http://autoregistration.codeplex.com/
    [TestFixture]
    public class UnityTest
    {
        private static bool InTargetAssembly(Type type)
        {
            return type.Assembly == typeof(BusinessProcess).Assembly;
        }

        private static bool IsLibraryAssembly(Assembly assembly)
        {
            return
                // NUnit is a 3rd party library
                (assembly == (typeof(TestFixtureAttribute).Assembly)) ||
                // Unity is a 3rd party library
                (assembly == (typeof(UnityContainer).Assembly));
        }

        [Test]
        public void CanMakeBusinessProcessWithAssemblyInclusion()
        {
            UnityContainer container = new UnityContainer();
            container.ConfigureAutoRegistration().
                // include just the target assembly
                Include(t => InTargetAssembly(t), 
                Then.Register().AsAllInterfacesOfType()).
                ApplyAutoRegistration();

            BusinessProcess bp = container.Resolve<BusinessProcess>();

            Assert.IsNotNull(bp);
        }

        [Test]
        public void CanMakeBusinessProcessWithAssemblyExclusion()
        {
            UnityContainer container = new UnityContainer();
            container.ConfigureAutoRegistration().
                Include(t => true,
                Then.Register().AsAllInterfacesOfType()).

                // exclude system and library assemblies
                ExcludeSystemAssemblies().
                ExcludeAssemblies(a => IsLibraryAssembly(a)).

                ApplyAutoRegistration();

            BusinessProcess bp = container.Resolve<BusinessProcess>();

            Assert.IsNotNull(bp);
        }

        [Test]
        public void CanGetAllValidators()
        {
            UnityContainer container = new UnityContainer();
            container.ConfigureAutoRegistration().
                Include(t => InTargetAssembly(t), 
                Then.Register().AsAllInterfacesOfType()).
                Include(If.Implements<IValidator>, Then.Register().As<IValidator>().WithTypeName()). 
                ApplyAutoRegistration();

            var validators = container.ResolveAll<IValidator>();

            Assert.IsNotNull(validators);
            Assert.AreEqual(3, validators.Count());
        }
    }
}
