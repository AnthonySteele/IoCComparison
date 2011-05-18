namespace IoCComparison
{
    using Ninject;
    using Ninject.Modules;
    using NUnit.Framework;

    /// <summary>
    /// Ninject, like Unity, only requires registrations from types were there is a mapping
    /// But the mappings are fixed and inside modules
    /// </summary>
    public class VanillaModule : NinjectModule 
    {
        public override void Load()
        {
            Bind<IJellybeanDispenser>().To<VanillaJellybeanDispenser>();
        }  
    }

    public class StrawberryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IJellybeanDispenser>().To<StrawberryJellybeanDispenser>();
        }
    }

    public class AniseedModule : NinjectModule
    {
        public override void Load()
        {
            Bind<SweetShop>().To<AniseedSweetShop>();
        }
    }

    public class SingletonServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IJellybeanDispenser>().To<VanillaJellybeanDispenser>().InSingletonScope();
        }
    }

    public class LemonModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IJellybeanDispenser>().To<AnyJellybeanDispenser>()
                .WithConstructorArgument("jellybean", Jellybean.Lemon);
        }
    }

    public class CocoaModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IJellybeanDispenser>().ToConstant(new AnyJellybeanDispenser(Jellybean.Cocoa));
        }
    }

    public class OrangeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IJellybeanDispenser>().ToMethod(c => new AnyJellybeanDispenser(Jellybean.Orange));
        }
    }

    [TestFixture]
    public class NInjectTestClass
    {
        [Test]
        public void CanMakeSweetShopWithVanillaJellybeans()
        {
            IKernel kernel = new StandardKernel(new VanillaModule());

            SweetShop sweetShop = kernel.Get<SweetShop>();

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithVanillaJellybeansWithoutModules()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IJellybeanDispenser>().To<VanillaJellybeanDispenser>();
            SweetShop sweetShop = kernel.Get<SweetShop>();

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithStrawberryJellybeans()
        {
            IKernel kernel = new StandardKernel(new StrawberryModule());

            SweetShop sweetShop = kernel.Get<SweetShop>();

            Assert.AreEqual(Jellybean.Strawberry, sweetShop.DispenseJellyBean());
        }
        
        [Test]
        public void JellybeanDispenserHasNewInstanceEachTime()
        {
            IKernel kernel = new StandardKernel(new VanillaModule());

            SweetShop sweetShop = kernel.Get<SweetShop>();
            SweetShop sweetShop2 = kernel.Get<SweetShop>();

            Assert.IsFalse(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine), "Contained objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser), "services are equal");
        }

        [Test]
        public void CanMakeSingletonJellybeanDispenser()
        {
            IKernel kernel = new StandardKernel(new SingletonServiceModule());

            SweetShop sweetShop = kernel.Get<SweetShop>();
            SweetShop sweetShop2 = kernel.Get<SweetShop>();

            Assert.IsFalse(ReferenceEquals(sweetShop, sweetShop2), "Root objects are equal");
            Assert.IsFalse(ReferenceEquals(sweetShop.SweetVendingMachine, sweetShop2.SweetVendingMachine), "Contained objects are equal");

            // should be same service
            Assert.IsTrue(ReferenceEquals(sweetShop.SweetVendingMachine.JellybeanDispenser, sweetShop2.SweetVendingMachine.JellybeanDispenser), "services are not equal");
        }
        
        [Test]
        public void CanMakeAniseedRootObject()
        {
            IKernel kernel = new StandardKernel(new AniseedModule());

            SweetShop sweetShop = kernel.Get<SweetShop>();

            Assert.AreEqual(Jellybean.Aniseed, sweetShop.DispenseJellyBean());
        }


        [Test]
        public void CanUseAnyJellybeanDispenser()
        {
            IKernel kernel = new StandardKernel(new LemonModule());

            SweetShop sweetShop = kernel.Get<SweetShop>();

            Assert.AreEqual(Jellybean.Lemon, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseConstructedObject()
        {
            IKernel kernel = new StandardKernel(new CocoaModule());

            SweetShop sweetShop = kernel.Get<SweetShop>();

            Assert.AreEqual(Jellybean.Cocoa, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseObjectFactory()
        {
            IKernel kernel = new StandardKernel(new OrangeModule());

            SweetShop sweetShop = kernel.Get<SweetShop>();

            Assert.AreEqual(Jellybean.Orange, sweetShop.DispenseJellyBean());
        }
    }
}
