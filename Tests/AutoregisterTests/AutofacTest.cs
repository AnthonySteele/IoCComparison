using System;
using AutoregisteredClasses.Validators;

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
        private static bool HasNoInterfaces(Type type)
        {
            return type.GetInterfaces().Length == 0;
        }

        [Test]
        public void CanMakeBusinessProcessTwoScans()
        {
            ContainerBuilder builder = new ContainerBuilder();

            // go through the target assembly twice 
            // - once for object without interfaces, once for those with interfaces
            builder.RegisterAssemblyTypes(typeof(BusinessProcess).Assembly).Where(t => HasNoInterfaces(t));
            builder.RegisterAssemblyTypes(typeof(CustomerService).Assembly).AsImplementedInterfaces();
            IContainer container = builder.Build();
            
            BusinessProcess bp = container.Resolve<BusinessProcess>();

            Assert.IsNotNull(bp);
        }

        [Test]
        public void CanMakeBusinessProcessOneScan()
        {
            ContainerBuilder builder = new ContainerBuilder();

            // instead of redundant scans, this version has redundant registrations
            // e.g. CustomerService is registered as both CustomerService and ICustomerService,
            // even though we only care about the latter
            builder.RegisterAssemblyTypes(typeof(CustomerService).Assembly).AsImplementedInterfaces().AsSelf();
            IContainer container = builder.Build();
            
            BusinessProcess bp = container.Resolve<BusinessProcess>();

            Assert.IsNotNull(bp);
        }

        [Test]
        public void CanMakeSingletonBusinessProcess()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(CustomerService).Assembly).AsImplementedInterfaces().AsSelf()
                .InstancePerLifetimeScope();
            IContainer container = builder.Build();

            BusinessProcess businessProcess1 = container.Resolve<BusinessProcess>();
            BusinessProcess businessProcess2 = container.Resolve<BusinessProcess>();

            Assert.AreSame(businessProcess1, businessProcess2);
        }

        [Test]
        public void CanMakeTransientBusinessProcess()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(CustomerService).Assembly).AsImplementedInterfaces().AsSelf();
            IContainer container = builder.Build();

            BusinessProcess businessProcess1 = container.Resolve<BusinessProcess>();
            BusinessProcess businessProcess2 = container.Resolve<BusinessProcess>();

            Assert.AreNotSame(businessProcess1, businessProcess2);
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

        [Test]
        public void CanFilterOutRegistrations()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IValidator).Assembly)
                .Where(t => t != typeof(FailValidator))
                .AsImplementedInterfaces();
            IContainer container = builder.Build();

            // excluding the FailValidator should leave 2 of them
            var validators = container.Resolve<IEnumerable<IValidator>>();

            Assert.IsNotNull(validators);
            Assert.AreEqual(2, validators.Count());
        }
    }
}
