namespace IoCComparison
{
    using System.ComponentModel.Composition.Hosting;

    using NUnit.Framework;

    [TestFixture]
    public class MEFTests2
    {
        [Test]
        public void CanMakeSweetShopWithVanillaJellybeans()
        {
            var catalog = new ConventionalCatalog();
            catalog.RegisterType<VanillaJellybeanDispenser, IJellybeanDispenser>();
            catalog.RegisterType<SweetVendingMachine>();
            catalog.RegisterType<SweetShop>();
            var container = new CompositionContainer(catalog);

            SweetShop sweetShop = container.GetExport<SweetShop>().Value;

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithStrawberryJellybeans()
        {
            var catalog = new ConventionalCatalog();
            catalog.RegisterType<StrawberryJellybeanDispenser, IJellybeanDispenser>();
            catalog.RegisterType<SweetVendingMachine>();
            catalog.RegisterType<SweetShop>();
            var container = new CompositionContainer(catalog);

            SweetShop sweetShop = container.GetExport<SweetShop>().Value;

            Assert.AreEqual(Jellybean.Strawberry, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeAniseedRootObject()
        {
            var catalog = new ConventionalCatalog();
            catalog.RegisterType<AniseedSweetShop, SweetShop>();
            var container = new CompositionContainer(catalog);
            SweetShop sweetShop = container.GetExport<SweetShop>().Value;

            Assert.AreEqual(Jellybean.Aniseed, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanUseAnyJellybeanDispenser()
        {
            SweetShop sweetShop = new SweetShop(new SweetVendingMachine(new AnyJellybeanDispenser(Jellybean.Strawberry)));

            Assert.AreEqual(Jellybean.Lemon, sweetShop.DispenseJellyBean());
        }
    }
}
