using AutoregisteredClasses.Validators;

namespace IoCComparison.AutoregisterTests
{
    using System.Linq;
    using AutoregisteredClasses.Interfaces;
    using AutoregisteredClasses.Services;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using NUnit.Framework;
    using IoCComparison.AutoregisterTests.NinjectExtensions;

    /// <summary>
    /// Need https://github.com/ninject/ninject.extensions.conventions
    /// </summary>
    [TestFixture]
    public class NinjectTest
    {
        [Test]
        public void CanMakeBusinessProcessWithDefaultBindingGenerator()
        {
            IKernel kernel = new StandardKernel();
            kernel.Scan(scanner =>
                {
                    scanner.From(typeof(BusinessProcess).Assembly);
                    scanner.BindWith<DefaultBindingGenerator>();
                });

            BusinessProcess bp = kernel.Get<BusinessProcess>();

            Assert.IsNotNull(bp);
        }

        [Test]
        public void CanMakeBusinessProcessWithServiceToInterfaceBinder()
        {
            IKernel kernel = new StandardKernel();
            kernel.Scan(scanner =>
            {
                scanner.From(typeof(BusinessProcess).Assembly);
                scanner.BindWith<NinjectServiceToInterfaceBinder>();
            });

            BusinessProcess bp = kernel.Get<BusinessProcess>();

            Assert.IsNotNull(bp);
        }

        [Test]
        public void CanMakeSingletonInstance()
        {
            IKernel kernel = new StandardKernel();
            kernel.Scan(scanner =>
            {
                scanner.From(typeof(BusinessProcess).Assembly);
                scanner.BindWith<NinjectServiceToInterfaceBinder>();
                scanner.InSingletonScope();
            });

            BusinessProcess businessProcess1 = kernel.Get<BusinessProcess>();
            BusinessProcess businessProcess2 = kernel.Get<BusinessProcess>();

            Assert.AreSame(businessProcess1, businessProcess2);
        }

        [Test]
        public void CanMakeTransientInstance()
        {
            IKernel kernel = new StandardKernel();
            kernel.Scan(scanner =>
            {
                scanner.From(typeof(BusinessProcess).Assembly);
                scanner.BindWith<NinjectServiceToInterfaceBinder>();
                scanner.InTransientScope();
            });

            BusinessProcess businessProcess1 = kernel.Get<BusinessProcess>();
            BusinessProcess businessProcess2 = kernel.Get<BusinessProcess>();

            Assert.AreNotSame(businessProcess1, businessProcess2);
        }

        [Test]
        public void CanMakeTransientInstanceWithSingletonDependencies()
        {
            IKernel kernel = new StandardKernel();
            kernel.Scan(scanner =>
            {
                scanner.From(typeof(BusinessProcess).Assembly);
                scanner.Where(t => t != typeof(BusinessProcess));
                scanner.BindWith<NinjectServiceToInterfaceBinder>();
                scanner.InSingletonScope();
            });
            BusinessProcess businessProcess1 = kernel.Get<BusinessProcess>();
            BusinessProcess businessProcess2 = kernel.Get<BusinessProcess>();

            Assert.AreNotSame(businessProcess1, businessProcess2);
            Assert.AreSame(businessProcess1.CustomerService, businessProcess2.CustomerService);
            Assert.AreSame(businessProcess1.OrderService, businessProcess2.OrderService);
        }

        [Test]
        public void CanGetAllValidators()
        {
            IKernel kernel = new StandardKernel();
            kernel.Scan(scanner =>
            {
                scanner.From(typeof(IValidator).Assembly);
                scanner.WhereTypeInheritsFrom<IValidator>();
                scanner.BindWith<NinjectServiceToInterfaceBinder>();
            });
            var validators = kernel.GetAll<IValidator>();

            Assert.IsNotNull(validators);
            Assert.AreEqual(3, validators.Count());
        }

        [Test]
        public void CanFilterOutValidatorRegistrations()
        {
            IKernel kernel = new StandardKernel();
            kernel.Scan(scanner =>
            {
                scanner.From(typeof(IValidator).Assembly);
                scanner.WhereTypeInheritsFrom<IValidator>();
                // excluding the FailValidator should leave 2 of them
                scanner.Where(t => t != typeof(FailValidator));
                scanner.BindWith<NinjectServiceToInterfaceBinder>();
            });
            var validators = kernel.GetAll<IValidator>();

            Assert.IsNotNull(validators);
            Assert.AreEqual(2, validators.Count());
        }

    }
}
