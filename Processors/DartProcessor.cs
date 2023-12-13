using System.Collections.Concurrent;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using DartSharp.Visitors;

namespace DartSharp.Processors
{
  abstract class DartProcessor
  {
    public IEnumerable<string> Files { get; }

    public DartProcessor(IEnumerable<string> files)
    {
      Files = files;
    }

    protected IDictionary<string, TDictValue> ProcessFiles<TVisitor, TDictValue>() where TVisitor : DartVisitorWrapper<TDictValue>, new()
    {
      var processResults = new ConcurrentDictionary<string, TDictValue>();
      Parallel.ForEach(Files, file =>
      {
        using TextReader text_reader = File.OpenText(file);

        var input = new AntlrInputStream(text_reader);

        var lexer = new Dart2Lexer(input);
        var tokens = new CommonTokenStream(lexer);
        var parser = new Dart2Parser(tokens)
        {
          BuildParseTree = true
        };

        IParseTree tree = parser.compilationUnit();

        var visitor = new TVisitor();
        var result = visitor.Visit(tree);

        var visitationResult = visitor.VisitationResult;
        processResults.TryAdd(file, visitationResult);
      });

      return processResults;
    }

    public abstract void Process();
  }
}
