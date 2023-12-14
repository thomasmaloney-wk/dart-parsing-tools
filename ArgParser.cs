namespace DartSharp
{
  enum ProgramMode
  {
    Garbage,
    PrintHelp,
    ExplodeMocks,
    ArgumentUseLint,
  }

  readonly struct ArgFlag
  {
    public readonly string Flag;
    public readonly string Description;

    public ArgFlag(string flag, string desc)
    {
      Flag = flag;
      Description = desc;
    }
  }

  readonly struct ArgParserResult
  {
    public readonly IEnumerable<string> Files;
    public readonly ProgramMode Mode;

    public ArgParserResult(ProgramMode mode, IEnumerable<string> files)
    {
      Mode = mode;
      Files = files;
    }
  }

  class ArgParser
  {
    private const string dirFlag = "--dir";
    private const string explodeFlag = "--explode";
    private const string lintFlag = "--lint";
    private const string helpFlag = "--help";
    private readonly List<ArgFlag> flags = new()
    {
      new(helpFlag, "Displays available options."),
      new(explodeFlag, "Writes all Mock classes to their own files"),
      new(lintFlag, "WIP: Will analyze a given dart file and notify if there are unused args for any functions"),
      new(dirFlag, "Consumes a directory instead of individual files and runs all dart files through a selected processor")
    };

    private List<string> flagParams => flags.Select((f) => f.Flag).ToList();

    private List<string> GetFilesFromDirectory(string directory)
    {
      return Directory.GetFiles(directory, "*.dart", SearchOption.AllDirectories).ToList();
    }

    public ArgParserResult ParseArguments(string[] args)
    {
      var files = new List<string>();
      var mode = ProgramMode.PrintHelp;

      if (args.Length == 0)
      {
        return new ArgParserResult(mode, files);
      }

      for (int i = 0; i < args.Length; i++)
      {
        if (args[i] == helpFlag)
          return new ArgParserResult(ProgramMode.PrintHelp, files);

        // todo: maybe write to stderr or something if more than one processor flag
        // is passed in?
        if (args[i] == explodeFlag)
        {
          mode = ProgramMode.ExplodeMocks;
          continue;
        }

        if (args[i] == lintFlag)
        {
          mode = ProgramMode.ArgumentUseLint;
          continue;
        }

        if (args[i] == dirFlag && i != args.Length - 1)
        {
          files = files.Union(GetFilesFromDirectory(args[++i])).ToList();
          continue;
        }

        if (args[i].EndsWith(".dart"))
        {
          files.Add(args[i]);
          continue;
        }

        // If we reach here, an invalid file or argument has been passed in
        // so set the mode to garbage and exit
        mode = ProgramMode.Garbage;
        // Hacky way to indicate which arg was invalid to the caller
        files = new() { args[i] };
        return new ArgParserResult(mode, files);
      }

      return new ArgParserResult(mode, files);
    }

    public void PrintHelp()
    {
      Console.WriteLine("Usage: DartCompiler [arguments] [files]");
      Console.WriteLine("  (or) dotnet run [arguments] [files]");
      Console.WriteLine("Arguments:");
      foreach (var flag in flags)
      {
        Console.WriteLine($"  {flag.Flag,-20} {flag.Description}");
      }
    }
  }
}
