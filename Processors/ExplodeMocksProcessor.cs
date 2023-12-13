using DartSharp.ResultPayloads;
using DartSharp.Visitors;

namespace DartSharp.Processors
{
  class ExplodeMocksProcessor : DartProcessor
  {
    public ExplodeMocksProcessor(IEnumerable<string> files) : base(files)
    {
    }

    public override void Process()
    {
      var results = ProcessFiles<DartClassVisitor, List<DartClassParseResult>>();

      Console.WriteLine($"Parsed {results.Count} files...");

      var profile = Environment.SpecialFolder.UserProfile;
      var home = Environment.GetFolderPath(profile);
      var mockDir = $"{home}/mock_files";
      foreach (var result in results)
      {

        Parallel.ForEach(result.Value, parseResult =>
        {
          var file = $"{mockDir}/{parseResult.ClassName.ToSnakeCase()}.dart";
          var content = parseResult.Text;
          Console.WriteLine($"Writing to {file}");

          try
          {
            using StreamWriter writer = new(file);
            writer.WriteLine("import 'package:mockito/mockito.dart';");
            writer.WriteLine("");
            writer.WriteLine(content);
          }
          catch (IOException ex)
          {
            Console.WriteLine($"[ERROR]: {ex.Message}");
          }
        });

        Console.WriteLine("========================");
      }
    }
  }
}
