using System.Collections.Generic;
using System.Linq;

namespace IoCComparison
{
    using NUnit.Framework;
    using LightCore;
    using LightCore.Lifecycle;

    [TestFixture]
    public class LightcoreTest
    {
        [Test]
        public void CanMakeSweetShopWithVanillaJellybeans()
        {
            var builder = new ContainerBuilder();
            builder.Register<IJellybeanDispenser, VanillaJellybeanDispenser>();
            builder.Register<SweetVendingMachine>();
            builder.Register<SweetShop>();

            IContainer container = builder.Build();
            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithVanillaJellybeansSweetshopNotRegistered()
        {
            var builder = new ContainerBuilder();
            builder.Register<IJellybeanDispenser, VanillaJellybeanDispenser>();
            builder.Register<SweetVendingMachine>();
            // builder.Register<SweetShop>() not needed, it has a no-param constructor

            IContainer container = builder.Build();
            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithStrawberryJellybeans()
        {
            var builder = new ContainerBuilder();
            builder.Register<IJellybeanDispenser, StrawberryJellybeanDispenser>();
            builder.Register<SweetVendingMachine>();
            builder.Register<SweetShop>();

            IContainer container = builder.Build();
            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Strawberry, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void ByDefaultAllObjectsHaveNewInstanceEachTime()
        {
            var builder = new ContainerBuilder();
            builder.Register<IJellybeanDispenser, VanillaJellybeanDispenser>();
            builder.Register<SweetVendingMachine>();
            builder.Register<SweetShop>();

            IContainer container = builder.Build();
            SweetShop sweetShop = container.Resolve<SweetShop>();
            SweetShop sweetShop2 = container.Resolve<SweetShop>();

            Assert.IsFalse(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine), "Contained objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser), "services are equal");
        }

        [Test]
        public void TopLevelObjectCanBeSingletonWithControlledBy()
        {
            var builder = new ContainerBuilder();
            builder.Register<IJellybeanDispenser, VanillaJellybeanDispenser>();
            builder.Register<SweetVendingMachine>();
            builder.Register<SweetShop>().ControlledBy<SingletonLifecycle>();

            IContainer container = builder.Build();
            SweetShop sweetShop = container.Resolve<SweetShop>();
            SweetShop sweetShop2 = container.Resolve<SweetShop>();

            Assert.IsTrue(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
        }

        [Test]
        public void BuilderDefaultControlledBySetsAllObjectsToSingleton()
        {
            var builder = new ContainerBuilder();
            builder.DefaultControlledBy<SingletonLifecycle>(); // do this before the register
            builder.Register<IJellybeanDispenser, VanillaJellybeanDispenser>();
            builder.Register<SweetVendingMachine>();
            builder.Register<SweetShop>();

            IContainer container = builder.Build();
            SweetShop sweetShop = container.Resolve<SweetShop>();
            SweetShop sweetShop2 = container.Resolve<SweetShop>();

            Assert.IsTrue(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
            Assert.IsTrue(ReferenceEquals(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine), "Contained objects are equal");
            Assert.IsTrue(ReferenceEquals(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser), "JellybeanDispenser are equal");
        }

        [Test]
        public void CanMakeSingletonJellybeanDispenser()
        {
            var builder = new ContainerBuilder();
            builder.Register<IJellybeanDispenser, VanillaJellybeanDispenser>()
                .ControlledBy<SingletonLifecycle>(); // is the default
            builder.Register<SweetVendingMachine>()
                .ControlledBy<TransientLifecycle>(); 
            builder.Register<SweetShop>();

            IContainer container = builder.Build();
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
            var builder = new ContainerBuilder();
            builder.Register<SweetShop, AniseedSweetShop>();

            IContainer container = builder.Build();
            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Aniseed, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseAnyJellybeanDispenser()
        {
            var builder = new ContainerBuilder();
            builder.Register<IJellybeanDispenser, AnyJellybeanDispenser>().WithArguments(Jellybean.Lemon);
            builder.Register<SweetVendingMachine>();
            builder.Register<SweetShop>();

            IContainer container = builder.Build();
            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Lemon, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseConstructedObject()
        {
            var builder = new ContainerBuilder();
            IJellybeanDispenser instance = new AnyJellybeanDispenser(Jellybean.Cocoa);
            // use a lambda to register the instance
            builder.Register(c => instance);
            builder.Register<SweetVendingMachine>();
            builder.Register<SweetShop>();

            IContainer container = builder.Build();
            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Cocoa, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseObjectFactory()
        {
            var builder = new ContainerBuilder();
            builder.Register<IJellybeanDispenser>(c => new AnyJellybeanDispenser(Jellybean.Orange));
            builder.Register<SweetVendingMachine>();
            builder.Register<SweetShop>();

            IContainer container = builder.Build();
            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Orange, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanRegisterMultipleDispensersWithResolveAll()
        {
            var builder = new ContainerBuilder();
            builder.Register<IJellybeanDispenser, VanillaJellybeanDispenser>();
            builder.Register<IJellybeanDispenser, StrawberryJellybeanDispenser>();

            IContainer container = builder.Build();
            IEnumerable<IJellybeanDispenser> dispensers = container.ResolveAll<IJellybeanDispenser>();

            Assert.IsNotNull(dispensers);
            Assert.AreEqual(2, dispensers.Count());
        }

        [Test]
        public void CanRegisterMultipleDispensersWithResolveIEnumerable()
        {
            var builder = new ContainerBuilder();
            builder.Register<IJellybeanDispenser, VanillaJellybeanDispenser>();
            builder.Register<IJellybeanDispenser, StrawberryJellybeanDispenser>();

            IContainer container = builder.Build();
            IEnumerable<IJellybeanDispenser> dispensers = container.Resolve<IEnumerable<IJellybeanDispenser>>();

            Assert.IsNotNull(dispensers);
            Assert.AreEqual(2, dispensers.Count());
        }
    }
}
