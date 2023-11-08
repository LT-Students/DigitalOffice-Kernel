using System;
using System.Text.RegularExpressions;

namespace DigitalOffice.Kernel.Helpers.TextHandlers;

public static class PasswordHider
{
  public static string Hide(string line)
  {
    string password = "Password";

    int index = line.IndexOf(password, 0, StringComparison.OrdinalIgnoreCase);

    if (index != -1)
    {
      string[] words = Regex.Split(line, @"[=,; ]");

      for (int i = 0; i < words.Length; i++)
      {
        if (string.Equals(password, words[i], StringComparison.OrdinalIgnoreCase))
        {
          line = line.Replace(words[i + 1], "****");
          break;
        }
      }
    }

    return line;
  }
}
