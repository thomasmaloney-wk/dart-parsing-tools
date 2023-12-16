using DartSharp.Processors;

namespace DartSharp.ArgumentHandling
{
    class CommandLineArgumentParsePayload
    {
        public DartProcessor? Processor => ProcessorFactory != null ? ProcessorFactory(Files, Output) : null;
        public List<string> Files { get; } = new();
        public Dictionary<string, IEnumerable<string>> GenericFlags = new();
        public List<string> Errors { get; } = new();

        public bool Verbose => GenericFlags.ContainsKey("--verbose");
        public bool ShowHelp => GenericFlags.ContainsKey("--help");
        public bool IsOutputLocationDefined => GenericFlags.ContainsKey("-o");
        public string? Output => IsOutputLocationDefined ? GenericFlags["-o"].FirstOrDefault(string.Empty) : null;

        public bool IsProcessorFlagSet => ProcessorFactory != null;

        private Func<IEnumerable<string>, string?, DartProcessor?>? ProcessorFactory;

        /// <summary>
        /// Tries to define how to create the dart processor.
        /// Fails if one is already defined.
        /// </summary>
        /// <param name="processorFactory"></param>
        /// <returns></returns>
        public bool TrySetProcessorFactory(Func<IEnumerable<string>, string?, DartProcessor?> processorFactory)
        {
            if (IsProcessorFlagSet) return false;
            ProcessorFactory = processorFactory;
            return true;
        }
    }
}