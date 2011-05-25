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

        /*
         * 
         docs: http://www.springframework.net/codeconfig/doc-latest/reference/html/
         This is a bit step forward for spring, it allows different configs to be used in different tests.
         The way that it works is a bit like NInject, but with advanced scan-the-app features that we don't need in this demo
         however it's more verbose and less typesafe than Ninject et al.
         And it has the problem that it doesn't do the basic de-coupling that is characteristic of IoC tools
         If a constructor changes (e.g. FooService now needs an IBarRepository) 
         The change has to be done in places other than the constructor itself.
         Eliminating that friction is what IoC is all about, and this doesn't do it.
         * */
    }
}
