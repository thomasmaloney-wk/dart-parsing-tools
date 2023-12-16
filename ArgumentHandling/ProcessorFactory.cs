using DartSharp.Processors;

namespace DartSharp.ArgumentHandling
{
    static class ProcessorFactory
    {
        public static TProcessor? CreateProcessor<TProcessor>(IEnumerable<string> files, string? outputParameter = null) where TProcessor : DartProcessor
        {
            try
            {
                if (outputParameter != null)
                {
                    return Activator.CreateInstance(typeof(TProcessor), files, outputParameter) as TProcessor;
                }
                return Activator.CreateInstance(typeof(TProcessor), files) as TProcessor;
            }
            catch (Exception)
            {
                // todo: maybe log the exception?
                return null;
            }
        }
    }
}