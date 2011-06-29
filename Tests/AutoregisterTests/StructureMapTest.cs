namespace IoCComparison.AutoregisterTests
{
    using System.Linq;
    using AutoregisteredClasses.Interfaces;
    using AutoregisteredClasses.Services;
    using AutoregisteredClasses.Validators;
    using IoCComparison.AutoregisterTests.StructureMapExtensions;
    using NUnit.Framework;
    using StructureMap;

    /// <summary>
    /// http://structuremap.net/structuremap/ScanningAssemblies.htm
    /// </summary>
    public class StructureMapTest
    {
        [Test]
        public void CanMakeBusinessProcess()
        {
            ObjectFactory.Initialize(x => 
                x.Scan(y =>
                    {
                        // y.RegisterConcreteTypesAgainstTheFirstInterface(); is equivalent to...
                        y.WithDefaultConventions();
                        y.AssemblyContainingType(typeof(BusinessProcess));
                    }));

            BusinessProcess businessProcess = ObjectFactory.GetInstance<BusinessProcess>();

            Assert.IsNotNull(businessProcess);
        }

        [Test]
        public void CanMakeSingletonInstance()
        {
            // must use a custom RegistrationConvention to set some as singletons
            // not ideal. Is there another way?
            CustomRegistrationConvention registrationConvention = new CustomRegistrationConvention()
                .WithSingleton<BusinessProcess>();

            ObjectFactory.Initialize(x =>
                    x.Scan(y =>
                    {
                        y.With(registrationConvention);
                        y.RegisterConcreteTypesAgainstTheFirstInterface();
                        y.AssemblyContainingType(typeof(BusinessProcess));
                    }));

            BusinessProcess businessProcess1 = ObjectFactory.GetInstance<BusinessProcess>();
            BusinessProcess businessProcess2 = ObjectFactory.GetInstance<BusinessProcess>();

            Assert.AreSame(businessProcess1, businessProcess2);
        }

        [Test]
        public void CanMakeTransientInstance()
        {
            ObjectFactory.Initialize(x =>
                x.Scan(y =>
                {
                    y.WithDefaultConventions();
                    y.AssemblyContainingType(typeof(BusinessProcess));
                }));

            BusinessProcess businessProcess1 = ObjectFactory.GetInstance<BusinessProcess>();
            BusinessProcess businessProcess2 = ObjectFactory.GetInstance<BusinessProcess>();

            Assert.AreNotSame(businessProcess1, businessProcess2);
        }

        [Test]
        public void CanMakeTransientInstanceWithSingletonDependencies()
        {
            CustomRegistrationConvention registrationConvention = new CustomRegistrationConvention()
                .WithSingleton<ICustomerService>().WithSingleton<IOrderService>();

            ObjectFactory.Initialize(x =>
                x.Scan(y =>
                {
                    y.With(registrationConvention);
                    y.AssemblyContainingType(typeof(BusinessProcess));
                }));


            BusinessProcess businessProcess1 = ObjectFactory.GetInstance<BusinessProcess>();
            BusinessProcess businessProcess2 = ObjectFactory.GetInstance<BusinessProcess>();

            Assert.AreNotSame(businessProcess1, businessProcess2);
            Assert.AreSame(businessProcess1.CustomerService, businessProcess2.CustomerService);
            Assert.AreSame(businessProcess1.OrderService, businessProcess2.OrderService);
        }


        [Test]
        public void CanGetAllValidators()
        {
            ObjectFactory.Initialize(
                x => x.Scan(
                y =>
                    {
                        y.AddAllTypesOf<IValidator>();
                        y.AssemblyContainingType(typeof(IValidator));
                    }));

            var validators = ObjectFactory.GetAllInstances<IValidator>();

            Assert.IsNotNull(validators);
            Assert.AreEqual(3, validators.Count());
        }

        [Test]
        public void CanFilterOutValidatorRegistrations()
        {
            ObjectFactory.Initialize(
                x => x.Scan(
                y =>
                {
                    y.AddAllTypesOf<IValidator>();
                    y.ExcludeType<FailValidator>();
                    y.AssemblyContainingType(typeof(IValidator));
                }));


            // excluding the FailValidator should leave 2 of them
            var validators = ObjectFactory.GetAllInstances<IValidator>();

            Assert.IsNotNull(validators);
            Assert.AreEqual(2, validators.Count());
        }
    }
}
