// See https://aka.ms/new-console-template for more information

using Antlr4.Runtime.Misc;

namespace DartSharp.Visitors
{
    class ParameterContainerTypeVisitor : DartVisitorWrapper<List<string>>
    {
        public List<string> things { get; set; } = new List<string>();
        public override List<string> VisitationResult { get { things.Add("i"); return things; } }

        public override int VisitLocalFunctionDeclaration([NotNull] Dart2Parser.LocalFunctionDeclarationContext context)
        {
            DoThing(context.functionSignature());
            things.Add($"things: {context.functionSignature()?.formalParameterPart()?.formalParameterList()?.ToString() ?? "no params"}");
            return 0;
        }

        public override int VisitTopLevelDeclaration([NotNull] Dart2Parser.TopLevelDeclarationContext context)
        {
            DoThing(context.functionSignature());
            return 0;
        }

        public override int VisitMethodSignature([NotNull] Dart2Parser.MethodSignatureContext context)
        {
            DoThing(context.functionSignature());
            things.Add($"things: {context.functionSignature()?.formalParameterPart()?.formalParameterList()?.ToString()}");
            return 0;

        }

        public override int VisitFormalParameterList([NotNull] Dart2Parser.FormalParameterListContext context)
        {
            Visit(context.normalFormalParameters());
            Visit(context.optionalOrNamedFormalParameters());
            return 0;
        }

        public override int VisitNormalFormalParameters([NotNull] Dart2Parser.NormalFormalParametersContext context)
        {
            //context.
            return 0;
        }

        private void DoThing(Dart2Parser.FunctionSignatureContext context)
        {
            if (context == null)
            {
                Console.WriteLine("context is null");
                return;
            }

            var formalParams = context.formalParameterPart();
            if (formalParams == null)
            {
                Console.WriteLine("formalParams is null");
                return;
            }

            var parameters = formalParams.formalParameterList().children;
            Visit(formalParams.formalParameterList());
            foreach (var p in parameters)
            {
                Console.WriteLine(p.ToStringTree());
            }
        }

    }
}
