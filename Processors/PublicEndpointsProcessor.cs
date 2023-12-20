
using DartSharp.Visitors;

namespace DartSharp.Processors
{
    class PublicEndpointsProcessor : DartProcessor
    {
        public PublicEndpointsProcessor(IEnumerable<string> files) : base(files)
        {
        }

        public override void Process()
        {
            var results = ProcessFiles<PublicEndpointsVisitor, Dictionary<string, List<string>>>();

            foreach (var result in results)
            {
                Console.WriteLine($"File {result.Key} contains the follow public classes with endpoints:");
                foreach (var @class in result.Value)
                {
                    Console.WriteLine($"Class identifier: {@class.Key}");
                    Console.WriteLine("Endpoints:");
                    foreach (var endpoint in @class.Value)
                        Console.WriteLine($"\t{endpoint}");
                }
            }
        }
    }
}
