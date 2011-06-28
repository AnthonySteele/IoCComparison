namespace IoCComparison.Classes
{
    /// <summary>
    /// This root object has no contained objects
    /// Used to test container mappings on the root object
    /// </summary>
    public class AniseedSweetShop : SweetShop
    {
        public AniseedSweetShop()
            : base(null)
        { }

        public override Jellybean DispenseJellyBean()
        {
            return Jellybean.Aniseed;
        }
    }

    /// <summary>
    /// Used to test constructor params that are a simple type - e.g. int, string, enum
    /// </summary>
    public class AnyJellybeanDispenser : IJellybeanDispenser
    {
        private readonly Jellybean jellybean;

        public AnyJellybeanDispenser(Jellybean jellybean)
        {
            this.jellybean = jellybean;
        }

        public Jellybean DispenseJellybean()
        {
            return jellybean;
        }
    }

}
