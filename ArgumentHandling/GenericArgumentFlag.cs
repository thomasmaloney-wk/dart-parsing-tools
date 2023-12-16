namespace DartSharp.ArgumentHandling
{
  /// <summary>
  /// Represents a program flag that is not associated with a specific
  /// <c>DartProcessor</c> class.
  /// </summary>
  class GenericArgumentFlag : IArgumentFlag
  {
    public int ParameterCount { get; }

    public string Flag { get; }

    public string Description { get; }

    public GenericArgumentFlag(string flag, string desc, int parameterCount = 0)
    {
      Flag = flag;
      Description = desc;
      ParameterCount = parameterCount;
    }
  }
}
