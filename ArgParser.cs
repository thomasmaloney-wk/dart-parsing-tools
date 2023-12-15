namespace DartSharp
{
  enum ProgramMode
  {
    Garbage,
    PrintHelp,
    ExplodeMocks,
    ArgumentUseLint,
    ListMocks,
    ListImports
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
    public readonly bool Verbose;
    public readonly ProgramMode Mode;

    public ArgParserResult(ProgramMode mode, bool verbose, IEnumerable<string> files)
    {
      Mode = mode;
      Verbose = verbose;
      Files = files;
    }
  }

  class ArgParser
  {
    private const string dirFlag = "--dir";
    private const string explodeFlag = "--explode";
    private const string lintFlag = "--lint";
    private const string verboseFlag = "--verbose";
    private const string helpFlag = "--help";
    private const string listMocksFlag = "--list-mocks";
    private const string listImportsFlag = "--list-imports";

    private readonly List<ArgFlag> flags = new()
    {
      new(helpFlag, "Displays available options. (Note: if running with dotnet run, to use this flag you'll need to call dotnet run -- --help)"),
      new(dirFlag, "Consumes a directory instead of individual files and runs all dart files through a selected processor. (Note: If your shell supports globs, you might not need to use this flag)"),
      new(explodeFlag, "Writes all Mock classes to their own files"),
      new(lintFlag, "WIP: Will analyze a given dart file and notify if there are unused args for any functions"),
      new(verboseFlag, "Print extra logging info while running this program. Useful for debugging."),
      new(listMocksFlag, "Print the Mock classes defined in the supplied files"),
      new(listImportsFlag, "Print the import statements in the supplied files")
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
      var verbose = false;

      if (args.Length == 0)
      {
        return new ArgParserResult(mode, verbose, files);
      }

      for (int i = 0; i < args.Length; i++)
      {
        if (args[i] == helpFlag)
          return new ArgParserResult(ProgramMode.PrintHelp, verbose, files);

        if (args[i] == verboseFlag)
        {
          verbose = true;
          continue;
        }

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

        if (args[i] == listMocksFlag) {
          mode = ProgramMode.ListMocks;
          continue;
        }

        if (args[i] == listImportsFlag) {
          mode = ProgramMode.ListImports;
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
        return new ArgParserResult(mode, verbose, files);
      }

      return new ArgParserResult(mode, verbose, files);
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
