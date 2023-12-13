
namespace DartSharp.Visitors
{
  abstract class DartVisitorWrapper<T> : Dart2ParserBaseVisitor<int>
  {
    public abstract T VisitationResult { get; }
  }
}
