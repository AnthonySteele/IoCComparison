namespace IoCComparison
{
    using NUnit.Framework;
    using Funq;

    [TestFixture]
    public class FunqTest
    {
        [Test]
        public void CanMakeSweetShopWithVanillaJellybeans()
        {
            Container container = new Container();
            container.Register<IJellybeanDispenser>(c => new VanillaJellybeanDispenser());
            container.Register(c => new SweetVendingMachine(c.Resolve<IJellybeanDispenser>()));
            container.Register(c => new SweetShop(c.Resolve<SweetVendingMachine>()));

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithStrawberryJellybeans()
        {
            Container container = new Container();
            container.Register<IJellybeanDispenser>(c => new StrawberryJellybeanDispenser());
            container.Register(c => new SweetVendingMachine(c.Resolve<IJellybeanDispenser>()));
            container.Register(c => new SweetShop(c.Resolve<SweetVendingMachine>()));
            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Strawberry, sweetShop.DispenseJellyBean());
        }

        /// <summary>
        /// This is not the test that I wanted, but it passes and thus describes funq's behaviour
        /// </summary>
        [Test]
        public void JellybeanDispenserHasSameInstanceEachTime()
        {
            Container container = new Container();
            container.Register<IJellybeanDispenser>(c => new StrawberryJellybeanDispenser());
            container.Register(c => new SweetVendingMachine(c.Resolve<IJellybeanDispenser>()));
            container.Register(c => new SweetShop(c.Resolve<SweetVendingMachine>()));

            SweetShop sweetShop = container.Resolve<SweetShop>();
            SweetShop sweetShop2 = container.Resolve<SweetShop>();

            Assert.IsTrue(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
            Assert.IsTrue(ReferenceEquals(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine), "Contained objects are equal");
            Assert.IsTrue(ReferenceEquals(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser), "services are equal");
        }

        [Test]
        public void CanMakeSingletonJellybeanDispenser()
        {
            Container container = new Container();
            container.Register<IJellybeanDispenser>(c => new StrawberryJellybeanDispenser());
            container.Register(c => new SweetVendingMachine(c.Resolve<IJellybeanDispenser>())).ReusedWithin(ReuseScope.None);
            container.Register(c => new SweetShop(c.Resolve<SweetVendingMachine>())).ReusedWithin(ReuseScope.None); 

            SweetShop sweetShop = container.Resolve<SweetShop>();
            SweetShop sweetShop2 = container.Resolve<SweetShop>();


            Assert.IsFalse(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine), "Contained objects are equal");

            // should be same service
            Assert.IsTrue(ReferenceEquals(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser), "services are not equal");
        }

        [Test]
        public void CanMakeAniseedRootObject()
        {
            Container container = new Container();
            container.Register<SweetShop>(c => new AniseedSweetShop()); 

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Aniseed, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseAnyJellybeanDispenser()
        {
            Container container = new Container();
            container.Register<IJellybeanDispenser>(c => new StrawberryJellybeanDispenser());
            container.Register(c => new SweetVendingMachine(c.Resolve<IJellybeanDispenser>()));
            container.Register(c => new SweetShop(c.Resolve<SweetVendingMachine>()));
            container.Register<IJellybeanDispenser>( c => new AnyJellybeanDispenser(Jellybean.Lemon));

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Lemon, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseConstructedObject()
        {

            Container container = new Container();
            container.Register<IJellybeanDispenser>(c => new StrawberryJellybeanDispenser());
            container.Register(c => new SweetVendingMachine(c.Resolve<IJellybeanDispenser>()));
            container.Register(c => new SweetShop(c.Resolve<SweetVendingMachine>()));

            IJellybeanDispenser instance = new AnyJellybeanDispenser(Jellybean.Cocoa);
            container.Register(c => instance);

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Cocoa, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseObjectFactory()
        {
            Container container = new Container();
            container.Register<IJellybeanDispenser>(c => new StrawberryJellybeanDispenser());
            container.Register(c => new SweetVendingMachine(c.Resolve<IJellybeanDispenser>()));
            container.Register(c => new SweetShop(c.Resolve<SweetVendingMachine>()));
            container.Register<IJellybeanDispenser>(c => new AnyJellybeanDispenser(Jellybean.Orange));

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Orange, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanRegisterMultipleDispensers()
        {

            Assert.Inconclusive("probably can't do this in Func() - " +
                "there's no method container.ResolveAll and resolving an IEnumerable<IJellybeanDispenser> doesn't work either");
        }
    }
}
