namespace DartSharp.ArgumentHandling
{
    class CommandLineArgumentParsePayload
    {
        public IEnumerable<string> Files { get; }
        public Dictionary<string, IEnumerable<string>> GenericFlags = new();

        public bool Verbose => GenericFlags.ContainsKey("--verbose");
        public bool ShowHelp => GenericFlags.ContainsKey("--help");
        public bool IsOutputLocationDefined => GenericFlags.ContainsKey("-o");
        public string? Output => IsOutputLocationDefined ? GenericFlags["-o"].FirstOrDefault(string.Empty) : null;

        public CommandLineArgumentParsePayload(IEnumerable<string> files)
        {
            Files = files;
        }

    }
}