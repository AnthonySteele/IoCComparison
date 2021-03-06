﻿namespace IoCComparison
{
    using System.Collections.Generic;
    using System.Linq;
    using IoCComparison.Classes;
    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    [TestFixture]
    public class UnityTestClass
    {
        [Test]
        public void CanMakeSweetShopWithVanillaJellybeans()
        {
            UnityContainer container = new UnityContainer();
            container.RegisterType<SweetShop>();
            container.RegisterType<SweetVendingMachine>();
            container.RegisterType<IJellybeanDispenser, VanillaJellybeanDispenser>();

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithVanillaJellybeansImplicitRegistration()
        {
            UnityContainer container = new UnityContainer();
            // Unity does not require explicit registrations for SweetShop and SweetVendingMachine
            // since there is no mapping, they are concrete types
            container.RegisterType<IJellybeanDispenser, VanillaJellybeanDispenser>();

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithVanillaJellybeansRepeatRegister()
        {
            UnityContainer container = new UnityContainer();
            // duplicate registration just overwrites
            container.RegisterType<IJellybeanDispenser, VanillaJellybeanDispenser>();
            container.RegisterType<IJellybeanDispenser, StrawberryJellybeanDispenser>();
            container.RegisterType<IJellybeanDispenser, VanillaJellybeanDispenser>();

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithStrawberryJellybeans()
        {
            UnityContainer container = new UnityContainer();
            container.RegisterType<IJellybeanDispenser, StrawberryJellybeanDispenser>();

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Strawberry, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void JellybeanDispenserHasNewInstanceEachTime()
        {
            UnityContainer container = new UnityContainer();
            container.RegisterType<IJellybeanDispenser, VanillaJellybeanDispenser>();

            SweetShop sweetShop = container.Resolve<SweetShop>();
            SweetShop sweetShop2 = container.Resolve<SweetShop>();

            Assert.AreNotSame(sweetShop, sweetShop2, "Root objects are equal");
            Assert.AreNotSame(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine, "Contained objects are equal");
            Assert.AreNotSame(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser, "services are equal");
        }

        [Test]
        public void CanMakeSingletonJellybeanDispenser()
        {            
            UnityContainer container = new UnityContainer();
            container.RegisterType<IJellybeanDispenser, StrawberryJellybeanDispenser>(new ContainerControlledLifetimeManager());

            SweetShop sweetShop = container.Resolve<SweetShop>();
            SweetShop sweetShop2 = container.Resolve<SweetShop>();

            Assert.AreNotSame(sweetShop, sweetShop2, "Root objects are equal");
            Assert.AreNotSame(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine, "Contained objects are equal");

            // should be same service
            Assert.AreSame(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser, "services are not equal");
        }

        [Test]
        public void CanMakeAniseedRootObject()
        {
            UnityContainer container = new UnityContainer();
            container.RegisterType<SweetShop, AniseedSweetShop>();

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Aniseed, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseAnyJellybeanDispenser()
        {
            UnityContainer container = new UnityContainer();
            container.RegisterType<IJellybeanDispenser, AnyJellybeanDispenser>(new InjectionConstructor(Jellybean.Lemon));

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Lemon, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseConstructedObject()
        {
            UnityContainer container = new UnityContainer();
            container.RegisterInstance<IJellybeanDispenser>(new AnyJellybeanDispenser(Jellybean.Cocoa));

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Cocoa, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseObjectFactory()
        {
            UnityContainer container = new UnityContainer();
            container.RegisterType<IJellybeanDispenser>(new InjectionFactory(c => new AnyJellybeanDispenser(Jellybean.Orange)));

            SweetShop sweetShop = container.Resolve<SweetShop>();

            Assert.AreEqual(Jellybean.Orange, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanRegisterMultipleDispensers()
        {
            UnityContainer container = new UnityContainer();
            // different registrations have different names, otherwise will overwrite
            container.RegisterType<IJellybeanDispenser, VanillaJellybeanDispenser>("vanilla");
            container.RegisterType<IJellybeanDispenser, StrawberryJellybeanDispenser>("strawberry");

            IEnumerable<IJellybeanDispenser> dispensers = container.ResolveAll<IJellybeanDispenser>();

            Assert.IsNotNull(dispensers);
            Assert.AreEqual(2, dispensers.Count());
			
        }
    }
}
