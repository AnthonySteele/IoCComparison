using AutoregisteredClasses.Interfaces;
using AutoregisteredClasses.Services;

namespace IoCComparison.AutoregisterTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Core;
    using Castle.Facilities.FactorySupport;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using NUnit.Framework;

    /// <summary>
    /// http://mikehadlow.blogspot.com/2010/01/10-advanced-windsor-tricks-2-auto.html
    /// </summary>
    [TestFixture]
    public class WindsorTest
    {
        [Test]
        public void CanMakeBusinessProcess()
        {
            WindsorContainer container = new WindsorContainer();
            container.Register(AllTypes.FromAssembly(typeof(BusinessProcess).Assembly).Pick()
                .WithService.DefaultInterface());

            BusinessProcess bp = container.Resolve<BusinessProcess>();

            Assert.IsNotNull(bp);
        }

        [Test]
        public void CanGetAllValidators()
        {
            WindsorContainer container = new WindsorContainer();
            container.Register(AllTypes.FromAssembly(typeof(IValidator).Assembly).Pick()
                .WithService.DefaultInterface());

            var validators = container.ResolveAll<IValidator>();

            Assert.IsNotNull(validators);
            Assert.AreEqual(3, validators.Count());
        }
    }
}
