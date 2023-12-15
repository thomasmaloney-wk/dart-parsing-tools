using DartSharp.Processors;

namespace DartSharp.ArgumentHandling
{
    /// <summary>
    /// Represents a program flag associated with a specific <c>DartProcessor</c>.
    /// </summary>
    /// <typeparam name="TProcessor">A subtype of <c>DartProcessor</c></typeparam>
    class ProcessorArgumentFlag<TProcessor> : ArgumentFlag where TProcessor : DartProcessor
    {
        public bool UsesOutputFlag { get; }
        public static Type ProcessorType => typeof(TProcessor);
        public override bool IsProcessorFlag => true;

        public ProcessorArgumentFlag(string flag, string desc, bool usesOutputFlag = false) : base(flag, desc)
        {
            UsesOutputFlag = usesOutputFlag;
        }

        public TProcessor CreateProcessor(IEnumerable<string> files, string? outputParameter)
        {
            if (UsesOutputFlag && outputParameter != null)
            {
                return (TProcessor)Activator.CreateInstance(typeof(TProcessor), files, outputParameter);
            }
            return (TProcessor)Activator.CreateInstance(typeof(TProcessor), files);
        }
    }
}