using System.Collections.Immutable;

namespace LT.DigitalOffice.Kernel.Constants;

public static class ImageFormats
{
  public const string jpg = ".jpg";
  public const string jpeg = ".jpeg";
  public const string png = ".png";
  public const string bmp = ".bmp";
  public const string gif = ".gif";
  public const string tga = ".tga";
  public const string svg = ".svg";
  public const string webp = ".webp";

  public static ImmutableList<string> formats = ImmutableList
    .Create(jpg, jpeg, png, bmp, gif, tga, svg, webp);
}
