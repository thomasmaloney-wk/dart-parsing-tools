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

    enum ProgramMode
    {
      Garbage,
      ExplodeMocks,
      ArgumentUseLint
    }

    public static int Main(string[] args)
    {
      var arguments = args.ToList();
      var mode = ProgramMode.Garbage;
      if (args.Length == 0)
      {
        Console.WriteLine("Error: Please provide at least one file.");
        return 1;
      }

      if (args.Contains("--explode"))
      {
        arguments.Remove("--explode");
        mode = ProgramMode.ExplodeMocks;
      }
      else if (args.Contains("--lint"))
      {
        arguments.Remove("--lint");
        mode = ProgramMode.ArgumentUseLint;
      }

      var files = GetFiles(arguments);
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
        case ProgramMode.Garbage:
          Console.WriteLine("Invalid mode");
          return 1;
      }

      processor?.Process();


      return 0;
    }
  }
}
