namespace SpringTests
{
    using IoCComparison.Classes;
    using Spring.Context;
    using Spring.Context.Support;
    using NUnit.Framework;

    /// <summary>
    /// Spring has been frustrating to set up 
    /// config file is undocumented and error-prone
    /// </summary>
    [TestFixture]
    public class SpringNoConfigTest
    {
        [Test]
        public void CanMakeSweetShopWithVanillaJellybeans()
        {
            GenericApplicationContext container = new GenericApplicationContext();
            container.RegisterType<SweetShop>("SweetShop");
            container.RegisterType<SweetVendingMachine>("SweetVendingMachine");
            container.RegisterType<VanillaJellybeanDispenser>("VanillaJellybeanDispenser");

            SweetShop sweetShop = (SweetShop)container.GetObject("SweetShop");

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithStrawberryJellybeans()
        {
            GenericApplicationContext container = new GenericApplicationContext();
            container.RegisterType<SweetShop>("SweetShop");
            container.RegisterType<SweetVendingMachine>("SweetVendingMachine");
            container.RegisterType<StrawberryJellybeanDispenser>("StrawberryJellybeanDispenser");
            SweetShop sweetShop = (SweetShop)container.GetObject("SweetShop");

            Assert.AreEqual(Jellybean.Strawberry, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void JellybeanDispenserHasNewInstanceEachTime()
        {
            GenericApplicationContext container = new GenericApplicationContext();
            container.RegisterType<SweetShop>("SweetShop");
            container.RegisterType<SweetVendingMachine>("SweetVendingMachine");
            container.RegisterType<VanillaJellybeanDispenser>("VanillaJellybeanDispenser");

            SweetShop sweetShop = (SweetShop)container.GetObject("SweetShop");
            SweetShop sweetShop2 = (SweetShop)container.GetObject("SweetShop");

            Assert.IsFalse(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine), "Contained objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser), "services are equal");
        }

        [Test]
        public void CanMakeSingletonJellybeanDispenser()
        {
            GenericApplicationContext container = new GenericApplicationContext();
            container.RegisterType<SweetShop>("SweetShop");
            container.RegisterType<SweetVendingMachine>("SweetVendingMachine");
            container.RegisterSingleton<VanillaJellybeanDispenser>("VanillaJellybeanDispenser");

            SweetShop sweetShop = (SweetShop)container.GetObject("SweetShop");
            SweetShop sweetShop2 = (SweetShop)container.GetObject("SweetShop");

            Assert.IsFalse(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine), "Contained objects are equal");

            // should be same service
            Assert.IsTrue(ReferenceEquals(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser), "services are not equal");
        }

        [Test]
        public void CanMakeAniseedRootObject()
        {
            GenericApplicationContext container = new GenericApplicationContext();
            container.RegisterType<AniseedSweetShop>("SweetShop");

            SweetShop sweetShop = (SweetShop)container.GetObject("SweetShop");

            Assert.AreEqual(Jellybean.Aniseed, sweetShop.DispenseJellyBean());
        }

        [Test]
        [Ignore("Spring is inflexible")]
        public void CanUseAnyJellybeanDispenser()
        {
            IApplicationContext ctx = ContextRegistry.GetContext();
            SweetShop sweetShop = (SweetShop)ctx.GetObject("SweetShop");

            Assert.AreEqual(Jellybean.Lemon, sweetShop.DispenseJellyBean());
        }


        [Test]
        [Ignore("Spring is inflexible")]
        public void CanUseConstructedObject()
        {
            IApplicationContext ctx = ContextRegistry.GetContext();
            SweetShop sweetShop = (SweetShop)ctx.GetObject("SweetShop");

            Assert.AreEqual(Jellybean.Cocoa, sweetShop.DispenseJellyBean());
        }

        [Test]
        [Ignore("Spring is inflexible")]
        public void CanUseFactoryMethod()
        {
            IApplicationContext ctx = ContextRegistry.GetContext();
            SweetShop sweetShop = (SweetShop)ctx.GetObject("SweetShop");

            Assert.AreEqual(Jellybean.Orange, sweetShop.DispenseJellyBean());
        }
    }
}
