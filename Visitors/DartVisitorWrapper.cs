// See https://aka.ms/new-console-template for more information

namespace DartSharp.Visitors
{
  abstract class DartVisitorWrapper<T> : Dart2ParserBaseVisitor<int>
  {
    public abstract T VisitationResult { get; }
  }
}
