
namespace IoCComparison.AutoregisterTests
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoregisteredClasses.Interfaces;
    using AutoregisteredClasses.Services;
    using Ninject;
    using Ninject.Modules;
    using NUnit.Framework;

    public class AutoRegisterModule : NinjectModule
    {
        public override void Load()
        {
            
        }
    }

    /// <summary>
    /// Need https://github.com/ninject/ninject.extensions.conventions
    /// </summary>
    [TestFixture]
    public class NinjectTest
    {
        [Test]
        public void CanMakeBusinessProcess()
        {
            IKernel kernel = new StandardKernel(new AutoRegisterModule());

            BusinessProcess bp = kernel.Get<BusinessProcess>();

            Assert.IsNotNull(bp);
        }

        [Test]
        public void CanGetAllValidators()
        {
            IKernel kernel = new StandardKernel(new AutoRegisterModule());

            var validators = kernel.GetAll<IValidator>();

            Assert.IsNotNull(validators);
            Assert.AreEqual(3, validators.Count());
        }

    }
}
