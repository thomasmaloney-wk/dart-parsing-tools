using DartSharp.ResultPayloads;
using DartSharp.Visitors;

namespace DartSharp.Processors
{
  class MockTypeDependenciesProcessor : DartProcessor
  {
    public MockTypeDependenciesProcessor(IEnumerable<string> files) : base(files)
    {
    }

    public override void Process()
    {
      var results = ProcessFiles<MockTypeDepsVisitor, MockTypeDependenciesParseResult>();
      Console.WriteLine($"Parsed {results.Count} files...");

      foreach (var result in results)
      {
        Console.WriteLine($"File {result.Key} contains the follow mocks:");
        foreach (var mock in result.Value.Mocks)
        {
          Console.WriteLine($"\t{mock}");
        }
        Console.WriteLine("========================");
      }
    }
  }
}
