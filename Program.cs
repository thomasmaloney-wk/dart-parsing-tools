using DartSharp.Processors;

namespace DartSharp
{
  class Program
  {

    public static List<string> GetFiles(List<string> args)
    {
      Console.WriteLine($"Provided {args.Count} arguments");
      var files = args.Where(arg => arg.EndsWith(".dart"));

      var dirArgIdx = args.IndexOf("--dir");
      if (dirArgIdx >= 0 && dirArgIdx < args.Count - 1)
      {
        Console.WriteLine($"Scanning directory: {args[dirArgIdx + 1]}");
        var extraFiles = Directory.GetFiles(args[dirArgIdx + 1], "*.dart", SearchOption.AllDirectories);
        if (extraFiles != null)
        {
          files = files.Union(extraFiles);
        }
      }

      return files.ToList();
    }

    public static int Main(string[] args)
    {
      var argParser = new ArgParser();
      var argResults = argParser.ParseArguments(args);
      var mode = argResults.Mode;
      var files = argResults.Files;
      var verbose = argResults.Verbose;

      if (verbose)
      {
        Console.WriteLine("ArgParse Results:");
        Console.WriteLine($"Mode: {argResults.Mode}");
        Console.WriteLine($"Files: {argResults.Files.Aggregate("", (x, y) => x + ", " + y)}");
      }

      if (mode == ProgramMode.PrintHelp)
      {
        argParser.PrintHelp();
        return 1;
      }
      else if (mode == ProgramMode.Garbage)
      {
        Console.WriteLine($"Invalid argument/file: {argResults.Files.First()}");
        argParser.PrintHelp();
        return 1;
      }

      DartProcessor? processor = null;
      switch (mode)
      {
        case ProgramMode.ExplodeMocks:
          processor = new ExplodeMocksProcessor(files);
          break;
        case ProgramMode.ArgumentUseLint:
          // Work in progress
          processor = new ParameterContainerTypeProcessor(files);
          break;
        default:
          break;
      }

      processor?.Process();


      return 0;
    }
  }
}
