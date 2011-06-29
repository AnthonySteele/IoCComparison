namespace IoCComparison.AutoregisterTests
{
    using AutoregisteredClasses.Interfaces;
    using AutoregisteredClasses.Services;
    using AutoregisteredClasses.Validators;
    using System.Linq;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using NUnit.Framework;

    [TestFixture]
    public class WindsorTest
    {
        [Test]
        public void CanMakeBusinessProcess()
        {
            WindsorContainer container = new WindsorContainer();
            container.Register(
                AllTypes.FromAssembly(typeof(BusinessProcess).Assembly).Pick()
                .WithService.DefaultInterface());

            BusinessProcess businessProcess = container.Resolve<BusinessProcess>();

            Assert.IsNotNull(businessProcess);
        }

        [Test]
        public void CanMakeSingletonInstance()
        {
            WindsorContainer container = new WindsorContainer();
            container.Register(AllTypes.FromAssembly(typeof(BusinessProcess).Assembly).Pick()
                .WithService.DefaultInterface());

            BusinessProcess businessProcess1 = container.Resolve<BusinessProcess>();
            BusinessProcess businessProcess2 = container.Resolve<BusinessProcess>();

            Assert.AreSame(businessProcess1, businessProcess2);
        }

        [Test]
        public void CanMakeTransientInstance()
        {
            WindsorContainer container = new WindsorContainer();
            container.Register(AllTypes.FromAssembly(typeof(BusinessProcess).Assembly).Pick()
                .Configure(component => component.LifeStyle.Transient)
                .WithService.DefaultInterface());

            BusinessProcess businessProcess1 = container.Resolve<BusinessProcess>();
            BusinessProcess businessProcess2 = container.Resolve<BusinessProcess>();

            Assert.AreNotSame(businessProcess1, businessProcess2);
        }

        [Test]
        public void CanMakeTransientInstanceWithSingletonDependencies()
        {
            WindsorContainer container = new WindsorContainer();
            container.Register(AllTypes.FromAssembly(typeof(BusinessProcess).Assembly).Pick()
                // do the ConfigureFor before the Configure, or else it fails
                .ConfigureFor<BusinessProcess>(component => component.LifeStyle.Transient)
                .Configure(component => component.LifeStyle.Singleton)
                .WithService.DefaultInterface());

            BusinessProcess businessProcess1 = container.Resolve<BusinessProcess>();
            BusinessProcess businessProcess2 = container.Resolve<BusinessProcess>();

            Assert.AreNotSame(businessProcess1, businessProcess2);
            Assert.AreSame(businessProcess1.CustomerService, businessProcess2.CustomerService);
            Assert.AreSame(businessProcess1.OrderService, businessProcess2.OrderService);
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

        [Test]
        public void CanFilterOutValidatorRegistrations()
        {
            WindsorContainer container = new WindsorContainer();
            container.Register(AllTypes.FromAssembly(typeof(IValidator).Assembly)
                .Where(t => t != typeof(FailValidator))
                .WithService.DefaultInterface());

            // excluding the FailValidator should leave 2 of them
            var validators = container.ResolveAll<IValidator>();

            Assert.IsNotNull(validators);
            Assert.AreEqual(2, validators.Count());
        }
    }
}
