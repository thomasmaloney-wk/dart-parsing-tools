using DartSharp.Processors;

namespace DartSharp.ArgumentHandling
{
    interface IProcessorArgumentFlag
    {
        public Type ProcessorType { get; }

        public DartProcessor CreateProcessor(IEnumerable<string> files, string? outputParameter = null);
    }
    /// <summary>
    /// Represents a program flag associated with a specific <c>DartProcessor</c>.
    /// </summary>
    /// <typeparam name="TProcessor">A subtype of <c>DartProcessor</c></typeparam>
    class ProcessorArgumentFlag<TProcessor> : ArgumentFlag, IProcessorArgumentFlag where TProcessor : DartProcessor
    {
        public bool UsesOutputFlag { get; }
        public Type ProcessorType => typeof(TProcessor);
        public override bool IsProcessorFlag => true;

        public ProcessorArgumentFlag(string flag, string desc, bool usesOutputFlag = false) : base(flag, desc)
        {
            UsesOutputFlag = usesOutputFlag;
        }

        public TProcessor CreateProcessor(IEnumerable<string> files, string? outputParameter = null)
        {
            // Todo: add error handling for the somewhat hacky reflection.
            if (UsesOutputFlag && outputParameter != null)
            {
                return (TProcessor)Activator.CreateInstance(typeof(TProcessor), files, outputParameter);
            }
            return (TProcessor)Activator.CreateInstance(typeof(TProcessor), files);
        }

        DartProcessor IProcessorArgumentFlag.CreateProcessor(IEnumerable<string> files, string? outputParameter)
        {
            return CreateProcessor(files, outputParameter: outputParameter);
        }
    }
}