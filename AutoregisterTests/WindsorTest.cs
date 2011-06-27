namespace IoCComparison.AutoregisterTests
{
    using AutoregisteredClasses.Interfaces;
    using AutoregisteredClasses.Services;
    using AutoregisteredClasses.Validators;
    using System.Linq;
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

            BusinessProcess businessProcess = container.Resolve<BusinessProcess>();

            Assert.IsNotNull(businessProcess);
        }

        [Test]
        public void CanMakeSingletonBusinessProcess()
        {
            WindsorContainer container = new WindsorContainer();
            container.Register(AllTypes.FromAssembly(typeof(BusinessProcess).Assembly).Pick()
                .WithService.DefaultInterface());

            BusinessProcess businessProcess1 = container.Resolve<BusinessProcess>();
            BusinessProcess businessProcess2 = container.Resolve<BusinessProcess>();

            Assert.AreEqual(businessProcess1, businessProcess2);
        }

        [Test]
        public void CanMakeTransientBusinessProcess()
        {
            WindsorContainer container = new WindsorContainer();
            container.Register(AllTypes.FromAssembly(typeof(BusinessProcess).Assembly).Pick()
                .Configure(component => component.LifeStyle.Transient)
                .WithService.DefaultInterface());

            BusinessProcess businessProcess1 = container.Resolve<BusinessProcess>();
            BusinessProcess businessProcess2 = container.Resolve<BusinessProcess>();

            Assert.AreNotEqual(businessProcess1, businessProcess2);
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
        public void CanFilterOutRegistrations()
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
