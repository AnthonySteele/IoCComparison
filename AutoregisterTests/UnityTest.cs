namespace IoCComparison.AutoregisterTests
{
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
        [Test]
        public void CanMakeBusinessProcess()
        {
            UnityContainer container = new UnityContainer();
            container.ConfigureAutoRegistration().
                LoadAssemblyFrom("AutoregisteredClasses.dll").
                Include(t => true,
                Then.Register().AsAllInterfacesOfType()).
                ApplyAutoRegistration();

            BusinessProcess bp = container.Resolve<BusinessProcess>();

            Assert.IsNotNull(bp);
        }

        [Test]
        public void CanGetAllValidators()
        {
            UnityContainer container = new UnityContainer();
            container.ConfigureAutoRegistration().
                LoadAssemblyFrom("AutoregisteredClasses.dll").
                Include(If.Implements<IValidator>, Then.Register().As<IValidator>().WithTypeName()). 
                ApplyAutoRegistration();

            var validators = container.ResolveAll<IValidator>();

            Assert.IsNotNull(validators);
            Assert.AreEqual(3, validators.Count());
        }
    }
}
