namespace IoCComparison
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class NoIoCTest
    {
        [Test]
        public void CanMakeSweetShopWithVanillaJellybeans()
        {
            SweetShop sweetShop = new SweetShop(new SweetVendingMachine(new VanillaJellybeanDispenser()));

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithStrawberryJellybeans()
        {
            SweetShop sweetShop = new SweetShop(new SweetVendingMachine(new StrawberryJellybeanDispenser()));

            Assert.AreEqual(Jellybean.Strawberry, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void JellybeanDispenserHasNewInstanceEachTime()
        {
            SweetShop sweetShop = new SweetShop(new SweetVendingMachine(new StrawberryJellybeanDispenser()));
            SweetShop sweetShop2 = new SweetShop(new SweetVendingMachine(new StrawberryJellybeanDispenser()));

            Assert.IsFalse(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine), "Contained objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser), "services are equal");
        }

        [Test]
        public void CanMakeSingletonJellybeanDispenser()
        {
            IJellybeanDispenser jellybeanDispenser = new StrawberryJellybeanDispenser();
            SweetShop sweetShop = new SweetShop(new SweetVendingMachine(jellybeanDispenser));
            SweetShop sweetShop2 = new SweetShop(new SweetVendingMachine(jellybeanDispenser));

            Assert.IsFalse(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine), "Contained objects are equal");

            // should be same service
            Assert.IsTrue(ReferenceEquals(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser), "services are not equal");
        }

        [Test]
        public void CanMakeAniseedRootObject()
        {
            SweetShop sweetShop = new AniseedSweetShop();

            Assert.AreEqual(Jellybean.Aniseed, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseAnyJellybeanDispenser()
        {
            SweetShop sweetShop = new SweetShop(new SweetVendingMachine(new AnyJellybeanDispenser(Jellybean.Lemon)));

            Assert.AreEqual(Jellybean.Lemon, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseConstructedObject()
        {
            IJellybeanDispenser dispenser = new AnyJellybeanDispenser(Jellybean.Cocoa);
            SweetShop sweetShop = new SweetShop(new SweetVendingMachine(dispenser));

            Assert.AreEqual(Jellybean.Cocoa, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseFactoryMethod()
        {
            Func<IJellybeanDispenser> factoryFunc = () => new AnyJellybeanDispenser(Jellybean.Orange);
            SweetShop sweetShop = new SweetShop(new SweetVendingMachine(factoryFunc()));

            Assert.AreEqual(Jellybean.Orange, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanRegisterMultipleDispensers()
        {
            IEnumerable<IJellybeanDispenser> dispensers = new List<IJellybeanDispenser>
                {
                    new VanillaJellybeanDispenser(),
                    new StrawberryJellybeanDispenser()
                };

            Assert.IsNotNull(dispensers);
            Assert.AreEqual(2, dispensers.Count());
        }
    }
}
