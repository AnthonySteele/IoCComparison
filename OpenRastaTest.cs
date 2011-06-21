namespace IoCComparison
{
    using NUnit.Framework;
    using OpenRasta.DI;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class OpenRastaTest
    {
        [Test]
        public void CanMakeSweetShopWithVanillaJellybeans()
        {
            IDependencyResolver container = new InternalDependencyResolver();
            container.AddDependency<SweetShop>();
            container.AddDependency<SweetVendingMachine>();
            container.AddDependency<IJellybeanDispenser, VanillaJellybeanDispenser>();

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithStrawberryJellybeans()
        {
            IDependencyResolver container = new InternalDependencyResolver();
            container.AddDependency<SweetShop>();
            container.AddDependency<SweetVendingMachine>();
            container.AddDependency<IJellybeanDispenser, StrawberryJellybeanDispenser>();

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Strawberry, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void JellybeanDispenserHasNewInstanceEachTime()
        {
            IDependencyResolver container = new InternalDependencyResolver();

            // default is " DependencyLifetime.Singleton" so specify transient objects
            container.AddDependency<SweetShop>(DependencyLifetime.Transient);
            container.AddDependency<SweetVendingMachine>(DependencyLifetime.Transient);
            container.AddDependency<IJellybeanDispenser, VanillaJellybeanDispenser>(DependencyLifetime.Transient);

            SweetShop sweetShop = container.Resolve<SweetShop>();
            SweetShop sweetShop2 = container.Resolve<SweetShop>();

            Assert.IsFalse(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine), "Contained objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser), "services are equal");
        }

        [Test]
        public void CanMakeSingletonJellybeanDispenser()
        {
            IDependencyResolver container = new InternalDependencyResolver();
            container.AddDependency<SweetShop>(DependencyLifetime.Transient);
            container.AddDependency<SweetVendingMachine>(DependencyLifetime.Transient);
            container.AddDependency<IJellybeanDispenser, StrawberryJellybeanDispenser>(DependencyLifetime.Singleton);

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
            IDependencyResolver container = new InternalDependencyResolver();
            container.AddDependency<SweetShop, AniseedSweetShop>();

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Aniseed, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseConstructedObject()
        {
            IDependencyResolver container = new InternalDependencyResolver();
            container.AddDependency<SweetShop>(DependencyLifetime.Transient);
            container.AddDependency<SweetVendingMachine>(DependencyLifetime.Transient);
            container.AddDependencyInstance<IJellybeanDispenser>(new AnyJellybeanDispenser(Jellybean.Cocoa));

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Cocoa, sweetShop.DispenseJellyBean());
        }

        /// <summary>
        /// This works on my fork of the OpenRasta container code
        /// </summary>
        [Test]
        public void CanUseObjectFactory()
        {
            IDependencyResolver container = new InternalDependencyResolver();
            container.AddDependency<SweetShop>(DependencyLifetime.Transient);
            container.AddDependency<SweetVendingMachine>(DependencyLifetime.Transient);
            container.AddDependency<IJellybeanDispenser>(c => new AnyJellybeanDispenser(Jellybean.Orange));

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Orange, sweetShop.DispenseJellyBean());
        }

        /// <summary>
        /// This works on my fork of the OpenRasta container code
        /// </summary>
        [Test]
        public void CanUseObjectFactoryWithSingleton()
        {
            IDependencyResolver container = new InternalDependencyResolver();
            container.AddDependency<SweetShop>(DependencyLifetime.Transient);
            container.AddDependency<SweetVendingMachine>(DependencyLifetime.Transient);

            IJellybeanDispenser instance = new AnyJellybeanDispenser(Jellybean.Orange);
            // The lambda captures the instance and uses it across multiple calls. It's a singleton
            container.AddDependency<IJellybeanDispenser>(c => instance);

            SweetShop sweetShop = container.Resolve<SweetShop>();
            SweetShop sweetShop2 = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Orange, sweetShop.DispenseJellyBean());
            Assert.AreEqual(Jellybean.Orange, sweetShop2.DispenseJellyBean());

            Assert.IsFalse(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine), "Contained objects are equal");

            // should be same service
            Assert.IsTrue(ReferenceEquals(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser), "services are not equal");
        }

        [Test]
        public void CanRegisterMultipleDispensers()
        {
            IDependencyResolver container = new InternalDependencyResolver();
            container.AddDependency<IJellybeanDispenser, VanillaJellybeanDispenser>();
            container.AddDependency<IJellybeanDispenser, StrawberryJellybeanDispenser>();

            IEnumerable<IJellybeanDispenser> dispensers = container.ResolveAll<IJellybeanDispenser>();

            Assert.IsNotNull(dispensers);
            Assert.AreEqual(2, dispensers.Count());
        }
    }
}
