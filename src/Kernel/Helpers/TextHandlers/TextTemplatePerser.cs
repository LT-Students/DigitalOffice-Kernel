using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Kernel.Helpers.TextHandlers
{
  public static class TextTemplatePerser
  {
    public static string Parse(Dictionary<string, string> values, string text)
    {
      string[] textArray = text.Split('{', '}');

      for (int i = 0; i < textArray.Length; i++)
      {
        textArray[i] =
            textArray[i].StartsWith('[') && textArray[i].EndsWith(']')
            && values.TryGetValue(textArray[i].Substring(1, textArray[i].Length - 2), out string value)
          ? value
          : ('{' + textArray[i] + '}');
      }

      return string.Join("", textArray);
    }

    public static string ParseModel<T>(T values, string text) where T : class
    {
      string[] textArray = text.Split('{', '}');

      Type type = values.GetType();

      for (int i = 0; i < textArray.Length; i++)
      {
        if (textArray[i].StartsWith('[') && textArray[i].EndsWith(']'))
        {
          textArray[i] = type
            .GetProperty(textArray[i].Substring(1, textArray[i].Length - 2))
            ?.GetValue(values)
            .ToString() ?? ('{' + textArray[i] + '}');
        }
      }

      return string.Join("", textArray);
    }
  }
}
