namespace IoCComparison.AutoregisterTests
{
    using System.Linq;
    using AutoregisteredClasses.Interfaces;
    using AutoregisteredClasses.Services;
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
                        y.WithDefaultConventions();
                        y.AssemblyContainingType(typeof(BusinessProcess));
                    }));

            BusinessProcess bp = ObjectFactory.GetInstance<BusinessProcess>();

            Assert.IsNotNull(bp);
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
    }
}
