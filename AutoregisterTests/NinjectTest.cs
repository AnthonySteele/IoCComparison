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

    }
}
