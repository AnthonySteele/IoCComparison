namespace IoCComparison
{
    using System.ComponentModel.Composition.Hosting;
    using NUnit.Framework;

    [TestFixture]
    public class MEFTests 
    {
        [Test]
        public void CanMakeSweetShopWithVanillaJellybeans()
        {
            TypeCatalog catalog = new TypeCatalog(typeof(VanillaJellybeanDispenser), typeof(SweetVendingMachine), typeof(SweetShop));
            CompositionContainer container = new CompositionContainer(catalog);

            SweetShop sweetShop = container.GetExport<SweetShop>().Value;

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        public void CanMakeSweetShopWithStrawberryJellybeans()
        {
            TypeCatalog catalog = new TypeCatalog(typeof(StrawberryJellybeanDispenser), typeof(SweetVendingMachine), typeof(SweetShop));
            CompositionContainer container = new CompositionContainer(catalog);

            SweetShop sweetShop = container.GetExport<SweetShop>().Value;

            Assert.AreEqual(Jellybean.Strawberry, sweetShop.DispenseJellyBean());
        }

        [Test]
        [Ignore("Does not work, AniseedSweetShop is not registered for SweetShop")]
        public void CanMakeAniseedRootObject()
        {
            TypeCatalog catalog = new TypeCatalog(typeof(AniseedSweetShop));
            CompositionContainer container = new CompositionContainer(catalog);

            SweetShop sweetShop = container.GetExport<SweetShop>().Value;

            Assert.AreEqual(Jellybean.Aniseed, sweetShop.DispenseJellyBean());
        }
    }
}
