namespace IoCComparison
{
    using Spring.Context;
    using Spring.Context.Support;
    using NUnit.Framework;

    /// <summary>
    /// Spring has been frustrating to set up 
    /// config file is undocumented and error-prone
    /// </summary>
    [TestFixture]
    public class SpringTest
    {
        [Test]
        public void CanMakeSweetShopWithVanillaJellybeans()
        {
            IApplicationContext ctx = ContextRegistry.GetContext();
            SweetShop sweetShop = (SweetShop)ctx.GetObject("SweetShop");

            Assert.AreEqual(Jellybean.Vanilla, sweetShop.DispenseJellyBean());
        }

        [Test]
        [Ignore("Spring is inflexible")]
        public void CanMakeSweetShopWithStrawberryJellybeans()
        {
            IApplicationContext ctx = ContextRegistry.GetContext();
            SweetShop sweetShop = (SweetShop)ctx.GetObject("SweetShop");

            Assert.AreEqual(Jellybean.Strawberry, sweetShop.DispenseJellyBean());
        }
    }
}
