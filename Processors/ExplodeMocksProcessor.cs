using DartSharp.ResultPayloads;
using DartSharp.Visitors;

namespace DartSharp.Processors
{
  class ExplodeMocksProcessor : DartProcessor
  {
    private readonly string outputDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/mock_files";
    public ExplodeMocksProcessor(IEnumerable<string> files) : base(files) { }
    public ExplodeMocksProcessor(IEnumerable<string> files, string? outputDirectory) : this(files)
    {
      if (outputDirectory != null)
      {
        this.outputDirectory = outputDirectory;
        Directory.CreateDirectory(this.outputDirectory);
      }
    }

    public override void Process()
    {
      var results = ProcessFiles<DartClassVisitor, List<DartClassParseResult>>();

      Console.WriteLine($"Parsed {results.Count} files...");

      foreach (var result in results)
      {

        Parallel.ForEach(result.Value, parseResult =>
        {
          var file = $"{outputDirectory}/{parseResult.ClassName.ToSnakeCase()}.dart";
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
