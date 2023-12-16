using DartSharp.Processors;

namespace DartSharp.ArgumentHandling
{
    interface IProcessorArgumentFlag : IArgumentFlag
    {
        public Type ProcessorType { get; }

        public DartProcessor? CreateProcessor(IEnumerable<string> files, string? outputParameter = null);
    }
    /// <summary>
    /// Represents a program flag associated with a specific <c>DartProcessor</c>.
    /// </summary>
    /// <typeparam name="TProcessor">A subtype of <c>DartProcessor</c></typeparam>
    class ProcessorArgumentFlag<TProcessor> : IProcessorArgumentFlag where TProcessor : DartProcessor
    {
        public bool UsesOutputFlag { get; }
        public Type ProcessorType => typeof(TProcessor);
        public bool IsProcessorFlag => true;

        public string Flag { get; }

        public string Description { get; }

        public ProcessorArgumentFlag(string flag, string desc, bool usesOutputFlag = false)
        {
            Flag = flag;
            Description = desc;
            UsesOutputFlag = usesOutputFlag;
        }

        DartProcessor? IProcessorArgumentFlag.CreateProcessor(IEnumerable<string> files, string? outputParameter)
        {
            if (UsesOutputFlag)
            {
                return ProcessorFactory.CreateProcessor<TProcessor>(files, outputParameter);
            }
            return ProcessorFactory.CreateProcessor<TProcessor>(files);
        }
    }
}