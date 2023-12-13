// See https://aka.ms/new-console-template for more information

namespace DartSharp.Visitors
{
  class ImportVisitor : DartVisitorWrapper<List<string>>
  {
    public List<string> imports { get; set; } = new List<string>();

    public override List<string> VisitationResult => imports;

    public override int VisitLibraryImport(Dart2Parser.LibraryImportContext ctx)
    {
      var metadata = ctx.metadata().GetText();
      var ims = ctx.importSpecification().configurableUri().uri().stringLiteral().singleLineString(0).GetText();
      var importStatement = metadata == null ? ims : $"{metadata} {ims}";
      imports.Add(importStatement);
      return 0;
    }
  }
}
