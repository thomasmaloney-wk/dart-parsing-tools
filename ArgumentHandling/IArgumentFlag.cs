namespace DartSharp.ArgumentHandling
{
  /// <summary>
  /// Represents an abstract program flag.
  /// </summary>
  interface IArgumentFlag
  {
    /// <summary>
    /// A string representing the flag passed into the program.
    /// Ex: "--help"
    /// </summary>
    public string Flag { get; }

    /// <summary>
    /// The description of the flag that gets printed when listing
    /// the available flags.
    /// 
    /// Ex: "Prints the help information"
    /// </summary>
    public string Description { get; }
  }
}