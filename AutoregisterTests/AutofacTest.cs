using System;

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

            // go through the target assembly twice 
            // - once for object without interfaces, once for those with interfaces
            // there's probably a better way to do this?
            builder.RegisterAssemblyTypes(typeof(BusinessProcess).Assembly).Where(t => HasNoInterfaces(t));
            builder.RegisterAssemblyTypes(typeof(CustomerService).Assembly).AsImplementedInterfaces();
            IContainer container = builder.Build();
            
            BusinessProcess bp = container.Resolve<BusinessProcess>();

            Assert.IsNotNull(bp);
        }

        private static bool HasNoInterfaces(Type type)
        {
            return type.GetInterfaces().Length == 0;
        }

        [Test]
        public void CanGetAllValidators()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IValidator).Assembly).AsImplementedInterfaces();
            IContainer container = builder.Build();

            var validators = container.Resolve<IEnumerable<IValidator>>();

            Assert.IsNotNull(validators);
            Assert.AreEqual(3, validators.Count());
        }
    }
}
