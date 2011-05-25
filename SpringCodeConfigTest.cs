using System;
using Spring.Context;
using Spring.Context.Support;

namespace IoCComparison
{
    using NUnit.Framework;
    using Spring.Context.Attributes;

    [Configuration]
    public class VanillaConfiguration
    {
        [Definition]
        public virtual SweetShop SweetShop()
        {
            return new SweetShop(SweetVendingMachine());
        }   
     
        [Definition]
        public virtual SweetVendingMachine SweetVendingMachine()
        {
            return new SweetVendingMachine(new VanillaJellybeanDispenser());
        }
    }

    [Configuration]
    public class StrawberryConfiguration
    {
        [Definition]
        public virtual SweetShop SweetShop()
        {
            return new SweetShop(SweetVendingMachine());
        }

        [Definition]
        public virtual SweetVendingMachine SweetVendingMachine()
        {
            return new SweetVendingMachine(new StrawberryJellybeanDispenser());
        }
    }

    [TestFixture]
    public class SpringCodeConfigTest
    {

        private static IApplicationContext CreateContextFromDefinition(Type definitionType)
        {
            CodeConfigApplicationContext context = new CodeConfigApplicationContext();
            context.ScanWithTypeFilter(t => t.Equals(definitionType));
            context.Refresh();
            return context;
        }

        [Test]
        public void CanMakeSweetShopWithVanillaJellybeans()
        {
            IApplicationContext ctx = CreateContextFromDefinition(typeof(VanillaConfiguration));
            SweetShop sweetShop = (SweetShop)ctx.GetObject("SweetShop");

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithStrawberryJellybeans()
        {
            IApplicationContext ctx = CreateContextFromDefinition(typeof(StrawberryConfiguration));
            SweetShop sweetShop = (SweetShop)ctx.GetObject("SweetShop");

            Assert.AreEqual(Jellybean.Strawberry, sweetShop.DispenseJellyBean());
        }
    }
}
