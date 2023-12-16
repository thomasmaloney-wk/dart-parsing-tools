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

        // Specify list type ArgumentFlag instead of ProcessorArgumentFlag since ProcessorArgumentFlag requires a generic type
        // which would restrict what processor related flags we can register.
        public static readonly List<ArgumentFlag> ProcessorFlags = new();

        public static Dictionary<string, GenericArgumentFlag> GenericFlagByFlagName => GenericFlags.ToDictionary(key => key.Flag, value => value);
        public static Dictionary<string, ArgumentFlag> ProcessorFlagByFlagName => ProcessorFlags.ToDictionary(key => key.Flag, value => value);

        public static IEnumerable<ArgumentFlag> Flags => ProcessorFlags.Union(GenericFlags);

        public static void RegisterFlag<TProcessor>(string flag, string desc, bool usesOutputFlag = false) where TProcessor : DartProcessor
        {
            ProcessorFlags.Add(new ProcessorArgumentFlag<TProcessor>(flag, desc, usesOutputFlag: usesOutputFlag));
        }

        public static void PrintHelp()
        {
            var genericFlags = GenericFlags;
            var processorFlags = ProcessorFlags;
            Console.WriteLine("Usage: DartCompiler [processor] [arguments] [files]");
            Console.WriteLine("  (or) dotnet run [processor] [arguments] [files]");
            Console.WriteLine("Processors:");
            foreach (var flag in processorFlags)
            {
                Console.WriteLine($"  {flag.Flag,-20} {flag.Description}");
            }
            Console.WriteLine("Arguments:");
            foreach (var flag in genericFlags)
            {
                Console.WriteLine($"  {flag.Flag,-20} {flag.Description}");
            }
        }
    }
}