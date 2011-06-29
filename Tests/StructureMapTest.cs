namespace IoCComparison
{
    using System.Collections.Generic;
    using System.Linq;
    using IoCComparison.Classes;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class StructureMapTest
    {
        [Test]
        public void CanMakeSweetShopWithVanillaJellybeans()
        {
            ObjectFactory.Initialize(
                x => x.For<IJellybeanDispenser>().Use<VanillaJellybeanDispenser>());

            SweetShop sweetShop = ObjectFactory.GetInstance<SweetShop>();

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithVanillaJellybeansWithContainer()
        {
            IContainer container = new Container(registry =>
                registry.For<IJellybeanDispenser>().Use<VanillaJellybeanDispenser>());

            SweetShop sweetShop = container.GetInstance<SweetShop>();

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithStrawberryJellybeans()
        {
            ObjectFactory.Initialize(
                x => x.For<IJellybeanDispenser>().Use<StrawberryJellybeanDispenser>());

            SweetShop sweetShop = ObjectFactory.GetInstance<SweetShop>();

            Assert.AreEqual(Jellybean.Strawberry, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void JellybeanDispenserHasNewInstanceEachTime()
        {
            ObjectFactory.Initialize(
                x => x.For<IJellybeanDispenser>().Use<VanillaJellybeanDispenser>());

            SweetShop sweetShop = ObjectFactory.GetInstance<SweetShop>();
            SweetShop sweetShop2 = ObjectFactory.GetInstance<SweetShop>();

            Assert.AreNotSame(sweetShop, sweetShop2, "Root objects are equal");
            Assert.AreNotSame(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine, "Contained objects are equal");
            Assert.AreNotSame(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser, "services are equal");
        }

        [Test]
        public void CanMakeSingletonJellybeanDispenser()
        {
            ObjectFactory.Initialize(
                x => x.ForSingletonOf<IJellybeanDispenser>().Use<VanillaJellybeanDispenser>());

            SweetShop sweetShop = ObjectFactory.GetInstance<SweetShop>();
            SweetShop sweetShop2 = ObjectFactory.GetInstance<SweetShop>();

            Assert.AreNotSame(sweetShop, sweetShop2, "Root objects are equal");
            Assert.AreNotSame(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine, "Contained objects are equal");

            // should be same service
            Assert.AreSame(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser, "services are not equal");
        }

        [Test]
        public void CanMakeAniseedRootObject()
        {
            ObjectFactory.Initialize(
                x => x.For<SweetShop>().Use<AniseedSweetShop>());

            SweetShop sweetShop = ObjectFactory.GetInstance<SweetShop>();

            Assert.AreEqual(Jellybean.Aniseed, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseAnyJellybeanDispenser()
        {
            ObjectFactory.Initialize(
                x => x.For<IJellybeanDispenser>().Use<AnyJellybeanDispenser>()
                .Ctor<Jellybean>("jellybean").Is(Jellybean.Lemon));

            SweetShop sweetShop = ObjectFactory.GetInstance<SweetShop>();

            Assert.AreEqual(Jellybean.Lemon, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseConstructedObject()
        {
            ObjectFactory.Initialize(
                     x => x.For<IJellybeanDispenser>().Use(new AnyJellybeanDispenser(Jellybean.Cocoa)));

            SweetShop sweetShop = ObjectFactory.GetInstance<SweetShop>();

            Assert.AreEqual(Jellybean.Cocoa, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseObjectFactory()
        {
            ObjectFactory.Initialize(
                    x => x.For<IJellybeanDispenser>().Use(c => new AnyJellybeanDispenser(Jellybean.Orange)));

            SweetShop sweetShop = ObjectFactory.GetInstance<SweetShop>();

            Assert.AreEqual(Jellybean.Orange, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanRegisterMultipleDispensers()
        {
            ObjectFactory.Initialize(x => 
                { 
                    x.For<IJellybeanDispenser>().Use<VanillaJellybeanDispenser>();
                    x.For<IJellybeanDispenser>().Use<StrawberryJellybeanDispenser>();
                });

            IEnumerable<IJellybeanDispenser> dispensers = ObjectFactory.GetAllInstances<IJellybeanDispenser>();

            Assert.IsNotNull(dispensers);
            Assert.AreEqual(2, dispensers.Count());
        }
    }
}
