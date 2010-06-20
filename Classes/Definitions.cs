namespace IoCComparison
{
    public enum Jellybean
    {
        Vanilla,
        Strawberry,
        Lemon,
        Orange,
        Aniseed,
        Cocoa
    };

    public interface IJellybeanDispenser
    {
        Jellybean DispenseJellybean();
    }
}
