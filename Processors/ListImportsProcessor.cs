using DartSharp.Visitors;

namespace DartSharp.Processors
{
  class ListImportsProcessor : DartProcessor
  {
    public ListImportsProcessor(IEnumerable<string> files) : base(files) { }

    public override void Process()
    {
      var results = ProcessFiles<ImportVisitor, List<string>>();
      Console.WriteLine($"Parsed {results.Count} files...");

      foreach (var result in results)
      {
        Console.WriteLine($"File {result.Key} contains the follow imports:");
        foreach (var import in result.Value)
        {
          Console.WriteLine($"\t{import}");
        }
        Console.WriteLine("========================");
      }
    }
  }
}
