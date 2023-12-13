// See https://aka.ms/new-console-template for more information

using System.Text;

namespace DartSharp
{
  static class Extensions
  {
    public static string ToSnakeCase(this string text)
    {
      if (text == null)
      {
        throw new ArgumentNullException(nameof(text));
      }
      if (text.Length < 2)
      {
        return text;
      }
      var sb = new StringBuilder();
      sb.Append(char.ToLowerInvariant(text[0]));
      for (int i = 1; i < text.Length; ++i)
      {
        char c = text[i];
        if (char.IsUpper(c))
        {
          sb.Append('_');
          sb.Append(char.ToLowerInvariant(c));
        }
        else
        {
          sb.Append(c);
        }
      }
      return sb.ToString();
    }
  }
}
