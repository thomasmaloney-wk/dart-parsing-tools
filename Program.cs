using DartSharp.ArgumentHandling;
using DartSharp.Processors;

namespace DartSharp
{
  class Program
  {
    public static void RegisterFlags()
    {
      FlagRegistry.RegisterFlag<ExplodeMocksProcessor>("--explode", "Writes all Mock classes to their own files");
      FlagRegistry.RegisterFlag<ParameterContainerTypeProcessor>("--lint", "WIP: Will analyze a given dart file and notify if there are unused args for any functions");
      FlagRegistry.RegisterFlag<MockTypeDependenciesProcessor>("--list-mocks", "Print the Mock classes defined in the supplied files");
      FlagRegistry.RegisterFlag<ListImportsProcessor>("--list-imports", "Print the import statements in the supplied files");
    }

    public static int Main(string[] args)
    {
      // temporarily hide old ArgParser behind undocumented flag
      if (args.Length > 0 && args[0] == "--legacy")
      {
        // remove --legacy arg from arg list before passing it on to legacy arg handler
        var argList = args.ToList();
        argList.Remove("--legacy");
        LegacyMain(argList.ToArray());
        return 0;
      }

      RegisterFlags();
      var argParserV2 = new CommandLineArgumentHandler();
      var resultPayload = argParserV2.ParseArguments(args);
      if (resultPayload.ShowHelp)
      {
        FlagRegistry.PrintHelpV2();
        return 0;
      }

      if (resultPayload.Errors.Any())
      {
        foreach (var error in resultPayload.Errors)
        {
          Console.WriteLine(error);
        }
        FlagRegistry.PrintHelpV2();
        return 1;
      }

      var processor = resultPayload.Processor;
      processor?.Process();

      return 0;
    }

    public static void LegacyMain(string[] args)
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
        return;
      }
      else if (mode == ProgramMode.Garbage)
      {
        Console.WriteLine($"Invalid argument/file: {argResults.Files.First()}");
        argParser.PrintHelp();
        return;
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
        case ProgramMode.ListMocks:
          processor = new MockTypeDependenciesProcessor(files);
          break;
        case ProgramMode.ListImports:
          processor = new ListImportsProcessor(files);
          break;
        default:
          break;
      }

      processor?.Process();
    }
  }
}
