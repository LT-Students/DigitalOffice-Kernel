using System.Collections.Immutable;

namespace LT.DigitalOffice.Kernel.Constants
{
    public static class ImageFormats
    {
        public const string jpgFormat = ".jpg";
        public const string jpegFormat = ".jpeg";
        public const string pngFormat = ".png";
        public const string bmpFormat = ".bmp";
        public const string gifFormat = ".gif";
        public const string tgaFormat = ".tga";
        public const string svgFormat = ".svg";

        public static ImmutableList<string> formats = ImmutableList
          .Create(jpgFormat, jpegFormat, pngFormat, bmpFormat, gifFormat, tgaFormat, svgFormat);
    }
}
