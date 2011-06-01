namespace IoCComparison
{
    using System;
    using Spring.Context;
    using Spring.Context.Support;
    using Spring.Objects.Factory.Support;
    using Spring.Context.Attributes;
    using NUnit.Framework;

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

    [Configuration]
    public class VanillaConfigurationWithNewInstances
    {
        [Definition]
        [Scope(ObjectScope.Prototype)]
        public virtual SweetShop SweetShop()
        {
            return new SweetShop(SweetVendingMachine());
        }

        [Definition]
        [Scope(ObjectScope.Prototype)]
        public virtual SweetVendingMachine SweetVendingMachine()
        {
            return new SweetVendingMachine(IJellybeanDispenser());
        }

        [Definition]
        [Scope(ObjectScope.Prototype)]
        public virtual IJellybeanDispenser IJellybeanDispenser()
        {
            return new StrawberryJellybeanDispenser();
        }
    }

    [Configuration]
    public class VanillaConfigurationWithSingletonJellybeanDispenser
    {
        [Definition]
        [Scope(ObjectScope.Prototype)]
        public virtual SweetShop SweetShop()
        {
            return new SweetShop(SweetVendingMachine());
        }

        [Definition]
        [Scope(ObjectScope.Prototype)]
        public virtual SweetVendingMachine SweetVendingMachine()
        {
            return new SweetVendingMachine(IJellybeanDispenser());
        }

        [Definition]
        [Scope(ObjectScope.Singleton)] // singleton scope is the default, but it is added here for emphasis
        public virtual IJellybeanDispenser IJellybeanDispenser()
        {
            return new StrawberryJellybeanDispenser();
        }
    }

    [Configuration]
    public class AniseedConfiguration
    {
        [Definition]
        public virtual SweetShop SweetShop()
        {
            return new AniseedSweetShop();
        }
    }

    [Configuration]
    public class LemonConfiguration
    {
        [Definition]
        public virtual SweetShop SweetShop()
        {
            return new SweetShop(SweetVendingMachine());
        }

        [Definition]
        public virtual SweetVendingMachine SweetVendingMachine()
        {
            return new SweetVendingMachine(new AnyJellybeanDispenser(Jellybean.Lemon));
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
            SweetShop sweetShop = ctx.GetObject<SweetShop>();

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithStrawberryJellybeans()
        {
            IApplicationContext ctx = CreateContextFromDefinition(typeof(StrawberryConfiguration));
            SweetShop sweetShop = ctx.GetObject<SweetShop>();

            Assert.AreEqual(Jellybean.Strawberry, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void JellybeanDispenserHasNewInstanceEachTime()
        {
            IApplicationContext ctx = CreateContextFromDefinition(typeof(VanillaConfigurationWithNewInstances));
            SweetShop sweetShop = ctx.GetObject<SweetShop>();
            SweetShop sweetShop2 = ctx.GetObject<SweetShop>();

            Assert.IsFalse(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine), "Contained objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser), "services are equal");
        }

        [Test]
        public void CanMakeSingletonJellybeanDispenser()
        {
            IApplicationContext ctx = CreateContextFromDefinition(typeof(VanillaConfigurationWithSingletonJellybeanDispenser));
            SweetShop sweetShop = ctx.GetObject<SweetShop>();
            SweetShop sweetShop2 = ctx.GetObject<SweetShop>();

            Assert.IsFalse(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine), "Contained objects are equal");

            // should be same service
            Assert.IsTrue(ReferenceEquals(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser), "services are not equal");
        }

        [Test]
        public void CanMakeAniseedRootObject()
        {
            IApplicationContext ctx = CreateContextFromDefinition(typeof(AniseedConfiguration));
            SweetShop sweetShop = ctx.GetObject<SweetShop>();

            Assert.AreEqual(Jellybean.Aniseed, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseAnyJellybeanDispenser()
        {
            IApplicationContext ctx = CreateContextFromDefinition(typeof(LemonConfiguration));
            SweetShop sweetShop = ctx.GetObject<SweetShop>();

            Assert.AreEqual(Jellybean.Lemon, sweetShop.DispenseJellyBean());
        }
    }
}
