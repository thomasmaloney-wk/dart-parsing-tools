using DartSharp.Processors;

namespace DartSharp.ArgumentHandling
{
    interface IProcessorArgumentFlag : IArgumentFlag
    {
        public DartProcessor? CreateProcessor(IEnumerable<string> files, string? outputParameter = null);
    }
}
