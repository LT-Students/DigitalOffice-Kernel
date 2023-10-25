using LTDO.Kernel.Helpers.TextHandlers.Interfaces;
using System;
using System.Collections.Generic;

namespace LTDO.Kernel.Helpers.TextHandlers;

public class TextTemplateParser : ITextTemplateParser
{
  public string Parse(Dictionary<string, string> values, string text)
  {
    string[] textArray = text.Split('{', '}');

    for (int i = 0; i < textArray.Length; i++)
    {
      if (textArray[i].StartsWith('[') && textArray[i].EndsWith(']'))
      {
        textArray[i] =
          values.TryGetValue(textArray[i].Substring(1, textArray[i].Length - 2), out string value)
          ? value
          : '{' + textArray[i] + '}';
      }
    }

    return string.Join("", textArray);
  }

  public string ParseModel<T>(T values, string text) where T : class
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
          .ToString() ?? '{' + textArray[i] + '}';
      }
    }

    return string.Join("", textArray);
  }
}
