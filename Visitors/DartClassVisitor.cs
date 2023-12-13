// See https://aka.ms/new-console-template for more information

using Antlr4.Runtime.Misc;
using DartSharp.ResultPayloads;

namespace DartSharp.Visitors
{
  class DartClassVisitor : DartVisitorWrapper<List<DartClassParseResult>>
  {
    public List<DartClassParseResult> Classes { get; } = new List<DartClassParseResult>();

    public override List<DartClassParseResult> VisitationResult => Classes;

    public override int VisitClassDeclaration([NotNull] Dart2Parser.ClassDeclarationContext context)
    {
      int a = context.Start.StartIndex;
      int b = context.Stop.StopIndex;
      Interval interval = new(a, b);
      var text = context.Start.InputStream.GetText(interval);
      Classes.Add(new DartClassParseResult(context.typeIdentifier().GetText(), text));
      return 0;
    }
  }
}
