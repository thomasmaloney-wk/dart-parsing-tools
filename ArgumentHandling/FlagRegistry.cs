using DartSharp.Processors;

namespace DartSharp.ArgumentHandling
{
    static class FlagRegistry
    {
        public static readonly List<GenericArgumentFlag> GenericFlags = new() {
            // prepopulate registry with non processor flags
            new("--help", "Displays available options. (Note: if running with dotnet run, to use this flag you'll need to call dotnet run -- --help)"),
            new("--dir", "Consumes a directory instead of individual files and runs all dart files through a selected processor. (Note: If your shell supports globs, you might not need to use this flag)", parameterCount: 1),
            new("--verbose", "Print extra logging info while running this program. Useful for debugging."),
            new("-o", "Writes output to specified directory for process commands that support it.", parameterCount: 1)
        };

        public static readonly List<IProcessorArgumentFlag> ProcessorFlags = new();

        public static Dictionary<string, GenericArgumentFlag> GenericFlagByFlagName => GenericFlags.ToDictionary(key => key.Flag, value => value);
        public static Dictionary<string, IProcessorArgumentFlag> ProcessorFlagByFlagName => ProcessorFlags.ToDictionary(key => key.Flag, value => value);

        public static void RegisterFlag<TProcessor>(string flag, string desc, bool usesOutputFlag = false) where TProcessor : DartProcessor
        {
            ProcessorFlags.Add(new ProcessorArgumentFlag<TProcessor>(flag, desc, usesOutputFlag: usesOutputFlag));
        }

        public static void PrintHelp()
        {
            Console.WriteLine("Usage: DartCompiler [processor] [arguments] [files]");
            Console.WriteLine("  (or) dotnet run [processor] [arguments] [files]");
            Console.WriteLine("Processors:");
            foreach (var flag in ProcessorFlags)
            {
                Console.WriteLine($"  {flag.Flag,-20} {flag.Description}");
            }
            Console.WriteLine("Arguments:");
            foreach (var flag in GenericFlags)
            {
                Console.WriteLine($"  {flag.Flag,-20} {flag.Description}");
            }
        }
    }
}