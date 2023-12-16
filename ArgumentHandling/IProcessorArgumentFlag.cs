using DartSharp.Processors;

namespace DartSharp.ArgumentHandling
{
    interface IProcessorArgumentFlag : IArgumentFlag
    {
        public Type ProcessorType { get; }

        public DartProcessor? CreateProcessor(IEnumerable<string> files, string? outputParameter = null);
    }
}