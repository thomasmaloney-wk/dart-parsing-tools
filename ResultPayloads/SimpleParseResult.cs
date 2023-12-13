// See https://aka.ms/new-console-template for more information

namespace DartSharp.ResultPayloads
{
  class SimpleParseResult
  {
    public string FileName { get; }
    public List<string> Imports { get; }

    public SimpleParseResult(string fileName, List<string> imports)
    {
      FileName = fileName;
      Imports = imports;
    }
  }
}
