namespace IoCComparison
{
    using System.ComponentModel.Composition;

    /// <summary>
    /// These classes are a very simple demo to simulate a real system's object tree
    /// The Root object owns SweetVendingMachine
    /// SweetVendingMachine owns the IJellybeanDispenser
    /// IJellybeanDispenser has two different implementations
    /// </summary>

    public enum Jellybean
    {
        Vanilla,
        Strawberry,
        Aniseed,
        Lemon
    } ;


    public interface IJellybeanDispenser
    {
        Jellybean DispenseJellybean();
    }

    [Export(typeof(IJellybeanDispenser))]
    public class VanillaJellybeanDispenser : IJellybeanDispenser
    {
        public Jellybean DispenseJellybean()
        {
            return Jellybean.Vanilla;
        }
    }

    [Export(typeof(IJellybeanDispenser))]
    public class StrawberryJellybeanDispenser : IJellybeanDispenser
    {
        public Jellybean DispenseJellybean()
        {
            return Jellybean.Strawberry;
        }
    }

    [Export]
    public class SweetVendingMachine
    {
        public IJellybeanDispenser JellybeanDispenser { get; private set; }

        [ImportingConstructor]
        public SweetVendingMachine([Import] IJellybeanDispenser jellybeanDispenser)
        {
            this.JellybeanDispenser = jellybeanDispenser;
        }
    }

    [Export]
    public class SweetShop
    {
        public SweetVendingMachine SweetVendingMachine { get; private set; }

        [ImportingConstructor]
        public SweetShop([Import] SweetVendingMachine sweetVendingMachine)
        {
            this.SweetVendingMachine = sweetVendingMachine;
        }

        public virtual Jellybean DispenseJellyBean()
        {
            return this.SweetVendingMachine.JellybeanDispenser.DispenseJellybean();
        }
    }

     /// <summary>
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
