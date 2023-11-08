using DigitalOffice.Kernel.Attributes;
using System.Collections.Generic;

namespace DigitalOffice.Kernel.Helpers.TextHandlers.Interfaces;

[AutoInject]
public interface ITextTemplateParser
{
  string Parse(Dictionary<string, string> values, string text);

  string ParseModel<T>(T values, string text) where T : class;
}
