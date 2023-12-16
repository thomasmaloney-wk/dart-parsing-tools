namespace DartSharp.ArgumentHandling
{
  /// <summary>
  /// Represents an abstract program flag.
  /// </summary>
  interface IArgumentFlag
  {
    public string Flag { get; }
    public string Description { get; }
    public bool IsProcessorFlag { get; }
  }
}