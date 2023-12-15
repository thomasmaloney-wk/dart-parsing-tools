namespace DartSharp.ArgumentHandling
{
  /// <summary>
  /// Represents a program flag that is not associated with a specific
  /// <c>DartProcessor</c> class.
  /// </summary>
  class GenericArgumentFlag : ArgumentFlag
  {
    public int ParameterCount { get; }
    public override bool IsProcessorFlag => false;

    public GenericArgumentFlag(string flag, string desc, int parameterCount = 0) : base(flag, desc)
    {
      ParameterCount = parameterCount;
    }
  }
}