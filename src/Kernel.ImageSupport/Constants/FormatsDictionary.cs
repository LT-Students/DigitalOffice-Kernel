using LT.DigitalOffice.Kernel.Constants;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tga;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace LT.DigitalOffice.Kernel.ImageSupport.Constants
{
  public static class FormatsDictionary
  {
    public static ImmutableDictionary<string, IImageFormat> formatsInstances = ImmutableDictionary
      .CreateRange(new Dictionary<string, IImageFormat>()
      {
        { ImageFormats.jpg, JpegFormat.Instance },
        { ImageFormats.jpeg, JpegFormat.Instance },
        { ImageFormats.png, PngFormat.Instance },
        { ImageFormats.bmp, BmpFormat.Instance },
        { ImageFormats.gif, GifFormat.Instance },
        { ImageFormats.tga, TgaFormat.Instance }
      });
  }
}
