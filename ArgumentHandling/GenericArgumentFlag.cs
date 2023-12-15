namespace DartSharp.ArgumentHandling
{
  /// <summary>
  /// Represents a program flag that is not associated with a specific
  /// <c>DartProcessor</c> class.
  /// </summary>
  class GenericArgumentFlag : ArgumentFlag
  {
    public int ParameterCount { get; }
    public IEnumerable<string> Parameters { get; }
    public override bool IsProcessorFlag => false;

    public GenericArgumentFlag(string flag, string desc, int parameterCount = 0, IEnumerable<string>? parameters = null) : base(flag, desc)
    {
      ParameterCount = parameterCount;
      Parameters = parameters ?? new List<string>();
    }
  }
}