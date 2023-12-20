using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace DartSharp.Visitors
{
    class PublicEndpointsVisitor : DartVisitorWrapper<Dictionary<string, List<string>>>
    {
        private readonly Dictionary<string, List<string>> results = new();
        public override Dictionary<string, List<string>> VisitationResult => results;

        private string? currentClass = null;

        private string GetContextText([NotNull] ParserRuleContext context)
        {
            int a = context.Start.StartIndex;
            int b = context.Stop.StopIndex;
            Interval interval = new(a, b);
            return context.Start.InputStream.GetText(interval);
        }

        public override int VisitClassDeclaration([NotNull] Dart2Parser.ClassDeclarationContext context)
        {
            Console.WriteLine($"called {nameof(VisitClassDeclaration)}");
            currentClass = context.typeIdentifier().IDENTIFIER().Symbol.Text;
            results[currentClass] = new();
            //results.Add(GetContextText(context));
            foreach (var child in context.children)
            {
                Visit(child);
            }
            return 0;
        }

        public override int VisitClassMemberDeclaration([NotNull] Dart2Parser.ClassMemberDeclarationContext context)
        {
            Console.WriteLine($"called {nameof(VisitClassMemberDeclaration)}");
            //results.Add(GetContextText(context));
            VisitChildren(context);
            return 0;
        }

        public override int VisitDeclaration([NotNull] Dart2Parser.DeclarationContext context)
        {
            Console.WriteLine($"called {nameof(VisitDeclaration)}");
            VisitChildren(context);
            results[currentClass].Add(GetContextText(context));
            return 0;
        }

        public override int VisitMethodSignature([NotNull] Dart2Parser.MethodSignatureContext context)
        {
            Console.WriteLine($"called {nameof(VisitMethodSignature)}");
            // if (context.constructorSignature())
            VisitChildren(context);
            results[currentClass].Add(GetContextText(context));
            return 0;
        }

        public override int VisitConstructorSignature([NotNull] Dart2Parser.ConstructorSignatureContext context)
        {
            Console.WriteLine($"called {nameof(VisitConstructorSignature)}");
            return 0;
        }

        public override int VisitFactoryConstructorSignature([NotNull] Dart2Parser.FactoryConstructorSignatureContext context)
        {
            Console.WriteLine($"called {nameof(VisitFactoryConstructorSignature)}");
            return 0;
        }

        public override int VisitFunctionSignature([NotNull] Dart2Parser.FunctionSignatureContext context)
        {
            Console.WriteLine($"called {nameof(VisitFunctionSignature)}");
            return 0;
        }

        public override int VisitGetterSignature([NotNull] Dart2Parser.GetterSignatureContext context)
        {
            Console.WriteLine($"called {nameof(VisitGetterSignature)}");
            return 0;
        }

        public override int VisitSetterSignature([NotNull] Dart2Parser.SetterSignatureContext context)
        {
            Console.WriteLine($"called {nameof(VisitSetterSignature)}");
            return 0;
        }

    }
}
