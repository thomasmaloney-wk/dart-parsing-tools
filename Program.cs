using DartSharp.ArgumentHandling;
using DartSharp.Processors;

namespace DartSharp
{
  class Program
  {
    public static void RegisterFlags()
    {
      FlagRegistry.RegisterFlag<ExplodeMocksProcessor>("--explode", "Writes all Mock classes to their own files", usesOutputFlag: true);
      FlagRegistry.RegisterFlag<ParameterContainerTypeProcessor>("--lint", "WIP: Will analyze a given dart file and notify if there are unused args for any functions");
      FlagRegistry.RegisterFlag<MockTypeDependenciesProcessor>("--list-mocks", "Print the Mock classes defined in the supplied files");
      FlagRegistry.RegisterFlag<ListImportsProcessor>("--list-imports", "Print the import statements in the supplied files");
    }

    public static int Main(string[] args)
    {
      RegisterFlags();
      var resultPayload = CommandLineArgumentHandler.ParseArguments(args);
      if (resultPayload.ShowHelp)
      {
        FlagRegistry.PrintHelp();
        return 0;
      }

      if (resultPayload.Errors.Any())
      {
        foreach (var error in resultPayload.Errors)
        {
          Console.WriteLine(error);
        }

        // print a newline before printing help message to help let error messages stand out more
        Console.WriteLine();
        FlagRegistry.PrintHelp();
        return 1;
      }

      var processor = resultPayload.Processor;
      processor?.Process();

      return 0;
    }
  }
}
