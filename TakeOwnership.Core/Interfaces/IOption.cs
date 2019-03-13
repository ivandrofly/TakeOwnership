namespace TakeOwnership.Interfaces
{
    public interface IOption
    {
        bool Directory { get; set; }
        bool File { get; set; }
        bool IsRecursive { get; set; }
        string Owner { get; set; }
        string Target { get; set; }
        bool Verbose { get; set; }
    }
}