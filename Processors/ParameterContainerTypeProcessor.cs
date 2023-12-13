using DartSharp.Visitors;

namespace DartSharp.Processors
{
    class ParameterContainerTypeProcessor : DartProcessor
    {
        public ParameterContainerTypeProcessor(IEnumerable<string> files) : base(files)
        {
        }

        public override void Process()
        {
            var results = ProcessFiles<ParameterContainerTypeVisitor, List<string>>();
            foreach (var key in results.Keys)
            {
                foreach (var val in results[key])
                {
                    Console.WriteLine($"{key}: {val}");
                }
            }
        }
    }
}
