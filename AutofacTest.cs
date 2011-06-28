namespace IoCComparison
{
    using IoCComparison.Classes;
    using System.Collections.Generic;
    using System.Linq;
    using Autofac;
    using NUnit.Framework;

    [TestFixture]
    public class AutofacTest
    {
        [Test]
        public void CanMakeSweetShopWithVanillaJellybeans()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<VanillaJellybeanDispenser>().AsImplementedInterfaces();
            builder.RegisterType<SweetVendingMachine>().AsSelf();
            builder.RegisterType<SweetShop>().AsSelf();
            IContainer container = builder.Build();

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithVanillaJellybeansAlternateSyntax()
        {
            ContainerBuilder builder = new ContainerBuilder();
            // use the more precise but verbose registration syntax of builder.RegisterType<T>().As<U>();
            builder.RegisterType<VanillaJellybeanDispenser>().As<IJellybeanDispenser>();
            builder.RegisterType<SweetVendingMachine>().As<SweetVendingMachine>();
            builder.RegisterType<SweetShop>().As<SweetShop>();
            IContainer container = builder.Build();

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }
        
        [Test]
        public void CanUpdateContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<VanillaJellybeanDispenser>().AsImplementedInterfaces();
            builder.RegisterType<SweetVendingMachine>().AsSelf();
            IContainer container = builder.Build();

            // the container is not actually immutable, you can update it like this:
            ContainerBuilder updater = new ContainerBuilder();
            updater.RegisterType<SweetShop>().AsSelf();
            updater.Update(container);

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithStrawberryJellybeans()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<StrawberryJellybeanDispenser>().AsImplementedInterfaces();
            builder.RegisterType<SweetVendingMachine>().AsSelf();
            builder.RegisterType<SweetShop>().AsSelf();
            IContainer container = builder.Build();


            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Strawberry, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void JellybeanDispenserHasNewInstanceEachTime()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<VanillaJellybeanDispenser>().AsImplementedInterfaces();
            builder.RegisterType<SweetVendingMachine>().AsSelf();
            builder.RegisterType<SweetShop>().AsSelf();
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
            builder.RegisterType<VanillaJellybeanDispenser>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<SweetVendingMachine>().AsSelf();
            builder.RegisterType<SweetShop>().AsSelf();
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
            builder.RegisterType<AnyJellybeanDispenser>().AsImplementedInterfaces()
                .WithParameter("jellybean", Jellybean.Lemon);

            builder.RegisterType<SweetVendingMachine>().AsSelf();
            builder.RegisterType<SweetShop>().AsSelf();
            IContainer container = builder.Build();

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Lemon, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseConstructedObject()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterInstance(new AnyJellybeanDispenser(Jellybean.Cocoa)).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<SweetVendingMachine>().AsSelf();
            builder.RegisterType<SweetShop>().AsSelf();

            IContainer container = builder.Build();
            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Cocoa, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseObjectFactory()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.Register(c => new AnyJellybeanDispenser(Jellybean.Orange)).AsImplementedInterfaces();
            builder.RegisterType<SweetVendingMachine>().AsSelf();
            builder.RegisterType<SweetShop>().AsSelf();

            IContainer container = builder.Build();
            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Orange, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanRegisterMultipleDispensers()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<VanillaJellybeanDispenser>().AsImplementedInterfaces();
            builder.RegisterType<StrawberryJellybeanDispenser>().AsImplementedInterfaces();
            IContainer container = builder.Build();

            // the Resolve<IEnumerable<>> syntax is the only way in autofac
            IEnumerable<IJellybeanDispenser> dispensers = container.Resolve<IEnumerable<IJellybeanDispenser>>();

            Assert.IsNotNull(dispensers);
            Assert.AreEqual(2, dispensers.Count());
        }
    }
}
