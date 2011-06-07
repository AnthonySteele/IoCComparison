namespace IoCComparison.AutoregisterTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Autofac;
    using AutoregisteredClasses.Interfaces;
    using AutoregisteredClasses.Services;
    using NUnit.Framework;

    [TestFixture]
    public class AutofacTest
    {
        [Test]
        public void CanMakeBusinessProcess()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(BusinessProcess).Assembly);
            IContainer container = builder.Build();
            
            // does not work, interface types not found
            BusinessProcess bp = container.Resolve<BusinessProcess>();

            Assert.IsNotNull(bp);
        }

        [Test]
        public void CanGetAllValidators()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IValidator).Assembly);
            IContainer container = builder.Build();

            var validators = container.Resolve<IEnumerable<IValidator>>();

            Assert.IsNotNull(validators);
            Assert.AreEqual(3, validators.Count());
        }
    }
}
