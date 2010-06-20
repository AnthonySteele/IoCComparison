using Autofac;
using NUnit.Framework;

namespace IoCComparison
{
    [TestFixture]
    public class AutofacTest
    {
        [Test]
        public void CanMakeSweetShopWithVanillaJellybeans()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<VanillaJellybeanDispenser>().As<IJellybeanDispenser>();
            builder.RegisterType<SweetVendingMachine>().As<SweetVendingMachine>();
            builder.RegisterType<SweetShop>().As<SweetShop>();
            IContainer container = builder.Build();

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithStrawberryJellybeans()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<StrawberryJellybeanDispenser>().As<IJellybeanDispenser>();
            builder.RegisterType<SweetVendingMachine>().As<SweetVendingMachine>();
            builder.RegisterType<SweetShop>().As<SweetShop>();
            IContainer container = builder.Build();


            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Strawberry, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void JellybeanDispenserHasNewInstanceEachTime()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<VanillaJellybeanDispenser>().As<IJellybeanDispenser>();
            builder.RegisterType<SweetVendingMachine>().As<SweetVendingMachine>();
            builder.RegisterType<SweetShop>().As<SweetShop>();
            IContainer container = builder.Build();

            SweetShop sweetShop = container.Resolve<SweetShop>();
            SweetShop sweetShop2 = container.Resolve<SweetShop>();

            Assert.IsFalse(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine), "Contained objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser), "services are equal");
        }

        [Test]
        public void CanMakeSingletonJellybeanDispenser()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<VanillaJellybeanDispenser>().As<IJellybeanDispenser>().SingleInstance();
            builder.RegisterType<SweetVendingMachine>().As<SweetVendingMachine>();
            builder.RegisterType<SweetShop>().As<SweetShop>();
            IContainer container = builder.Build();

            SweetShop sweetShop = container.Resolve<SweetShop>();
            SweetShop sweetShop2 = container.Resolve<SweetShop>();

            Assert.IsFalse(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine), "Contained objects are equal");

            // should be same instance
            Assert.IsTrue(ReferenceEquals(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser), "services are not equal");
        }

        [Test]
        public void CanMakeAniseedRootObject()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<AniseedSweetShop>().As<SweetShop>();
            IContainer container = builder.Build();

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Aniseed, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseAnyJellybeanDispenser()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<AnyJellybeanDispenser>().As<IJellybeanDispenser>()
                .WithParameter("jellybean", Jellybean.Lemon);

            builder.RegisterType<SweetVendingMachine>().As<SweetVendingMachine>();
            builder.RegisterType<SweetShop>().As<SweetShop>();
            IContainer container = builder.Build();

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Lemon, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseConstructedObject()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterInstance(new AnyJellybeanDispenser(Jellybean.Cocoa)).As<IJellybeanDispenser>().SingleInstance();
            builder.RegisterType<SweetVendingMachine>().As<SweetVendingMachine>();
            builder.RegisterType<SweetShop>().As<SweetShop>();

            IContainer container = builder.Build();
            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Cocoa, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseObjectFactory()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.Register(c => new AnyJellybeanDispenser(Jellybean.Orange)).As<IJellybeanDispenser>();
            builder.RegisterType<SweetVendingMachine>().As<SweetVendingMachine>();
            builder.RegisterType<SweetShop>().As<SweetShop>();

            IContainer container = builder.Build();
            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Orange, sweetShop.DispenseJellyBean());
        }
    }
}
