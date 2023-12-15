namespace DartSharp.ArgumentHandling
{
  /// <summary>
  /// Represents an abstract program flag.
  /// </summary>
  abstract class ArgumentFlag
  {
    public string Flag { get; }
    public string Description { get; }
    public abstract bool IsProcessorFlag { get; }

    protected ArgumentFlag(string flag, string desc)
    {
      Flag = flag;
      Description = desc;
    }
  }
}