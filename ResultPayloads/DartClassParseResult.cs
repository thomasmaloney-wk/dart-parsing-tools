namespace DartSharp.ResultPayloads
{
  class DartClassParseResult
  {
    public string ClassName { get; }
    public string Text { get; }

    public DartClassParseResult(string className, string text)
    {
      ClassName = className;
      Text = text;
    }
  }
}
