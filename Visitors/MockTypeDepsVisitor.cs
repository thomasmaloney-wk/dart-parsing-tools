using Antlr4.Runtime.Misc;
using DartSharp.ResultPayloads;

namespace DartSharp.Visitors
{
    class MockTypeDepsVisitor : DartVisitorWrapper<MockTypeDependenciesParseResult>
    {
        private readonly MockTypeDependenciesParseResult result = new();

        public override MockTypeDependenciesParseResult VisitationResult => result;

        public override int VisitDeclaredIdentifier([NotNull] Dart2Parser.DeclaredIdentifierContext context)
        {
            var identifier = context.finalConstVarOrType().GetText();
            if (identifier == null || !identifier.StartsWith("Mock"))
                return 0;

            result.Mocks.Add(identifier);
            return 0;
        }
    }
}
