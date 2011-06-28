namespace IoCComparison.Classes
{
    public class VanillaJellybeanDispenser : IJellybeanDispenser
    {
        public Jellybean DispenseJellybean()
        {
            return Jellybean.Vanilla;
        }
    }

    public class StrawberryJellybeanDispenser : IJellybeanDispenser
    {
        public Jellybean DispenseJellybean()
        {
            return Jellybean.Strawberry;
        }
    }

    public class SweetVendingMachine
    {
        public IJellybeanDispenser JellybeanDispenser { get; private set; }

        public SweetVendingMachine(IJellybeanDispenser jellybeanDispenser)
        {
            this.JellybeanDispenser = jellybeanDispenser;
        }
    }

    public class SweetShop
    {
        public SweetVendingMachine SweetVendingMachine { get; private set; }

        public SweetShop(SweetVendingMachine sweetVendingMachine)
        {
            this.SweetVendingMachine = sweetVendingMachine;
        }

        public virtual Jellybean DispenseJellyBean()
        {
            return this.SweetVendingMachine.JellybeanDispenser.DispenseJellybean();
        }
    }
}
