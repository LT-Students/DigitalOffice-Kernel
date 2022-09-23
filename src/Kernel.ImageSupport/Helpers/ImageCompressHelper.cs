using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using DigitalOffice.Kernel.ImageSupport.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Svg;
using Image = SixLabors.ImageSharp.Image;

namespace DigitalOffice.Kernel.ImageSupport.Helpers
{
  public class ImageCompressHelper : IImageCompressHelper
  {
    private readonly ILogger<ImageCompressHelper> _logger;

    public const string png = ".png";
    public const string svg = ".svg";
    public const string jpeg = ".jpeg";

    public ImageCompressHelper(ILogger<ImageCompressHelper> logger)
    {
      _logger = logger;
    }

    public Task<(bool isSuccess, string compressedContent, string extension)> CompressAsync(string inputBase64, string extension, int MaxWeighInKB)
    {
      return Task.Run(() =>
      {
        try
        {
          string compressedContent = inputBase64;
          int quality = 90;

          while (Convert.FromBase64String(compressedContent).Length / 1000 > MaxWeighInKB)
          {
            if (extension == svg)
            {
              byte[] byteString = Convert.FromBase64String(inputBase64);
              using MemoryStream ms = new MemoryStream(byteString);
              Bitmap bmp = SvgDocument.Open<SvgDocument>(ms).Draw();
              ImageConverter converter = new ImageConverter();
              byteString = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
              compressedContent = Convert.ToBase64String(byteString);
              extension = png;
            }
            else if (extension == jpeg)
            {
              Configuration.Default.ImageFormatsManager.SetEncoder(JpegFormat.Instance, new JpegEncoder()
              {
                Quality = quality
              });

              Image image = Image.Load(Convert.FromBase64String(compressedContent), out IImageFormat imageFormat);
              compressedContent = image.ToBase64String(imageFormat).Split(',')[1];
              quality -= 10;

              if (quality == 0)
              {
                break;
              }
            }
            else
            {
              Image image = Image.Load(Convert.FromBase64String(inputBase64));
              image.Mutate(x => x.BackgroundColor(SixLabors.ImageSharp.Color.White));
              compressedContent = image.ToBase64String(JpegFormat.Instance).Split(',')[1];
              extension = jpeg;
            }
          }
          return (isSuccess: true, compressedContent, extension);
        }
        catch (Exception ex)
        {
          _logger.LogWarning("Can't compress image: content is damaged or format is wrong. " + ex.Message);
          return (isSuccess: false, compressedContent: null, extension);
        }
      });
    }
  }
}
