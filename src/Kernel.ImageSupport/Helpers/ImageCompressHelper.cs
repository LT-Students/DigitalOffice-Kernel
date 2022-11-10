using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using DigitalOffice.Kernel.ImageSupport.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Svg;
using Image = SixLabors.ImageSharp.Image;

namespace DigitalOffice.Kernel.ImageSupport.Helpers;

public class ImageCompressHelper : IImageCompressHelper
{
  private readonly ILogger<ImageCompressHelper> _logger;

  private string ConvertSvgToPng(string inputContent)
  {
    byte[] byteString = Convert.FromBase64String(inputContent);
    using MemoryStream ms = new MemoryStream(byteString);
    Bitmap bmp = SvgDocument.Open<SvgDocument>(ms).Draw();
    ImageConverter converter = new ImageConverter();
    byteString = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
    string outputContent = Convert.ToBase64String(byteString);
    return outputContent;
  }

  private string CompressJpeg(string inputContent, int quality)
  {
    Configuration.Default.ImageFormatsManager.SetEncoder(JpegFormat.Instance, new JpegEncoder
    {
      Quality = quality
    });

    Image image = Image.Load(Convert.FromBase64String(inputContent), out IImageFormat imageFormat);
    string outputContent = image.ToBase64String(imageFormat).Split(',')[1];
    return outputContent;
  }

  private string ConvertPixelTypesToJpeg(string inputContent)
  {
    Image image = Image.Load(Convert.FromBase64String(inputContent));
    image.Mutate(x => x.BackgroundColor(SixLabors.ImageSharp.Color.White));
    string outputContent = image.ToBase64String(JpegFormat.Instance).Split(',')[1];
    return outputContent;
  }

  public ImageCompressHelper(ILogger<ImageCompressHelper> logger)
  {
    _logger = logger;
  }

  public Task<(bool isSuccess, string compressedContent, string extension)> CompressAsync(string inputBase64, string extension, int maxSizeKb)
  {
    return Task.Run(() =>
    {
      try
      {
        string compressedContent = inputBase64;
        int quality = 90;

        while (Convert.FromBase64String(compressedContent).Length / 1000 > maxSizeKb && quality > 0)
        {
          switch (extension)
          {
            case ImageFormats.svg:
              compressedContent = ConvertSvgToPng(compressedContent);
              extension = ImageFormats.png;
              break;

            case ImageFormats.jpeg:
              compressedContent = CompressJpeg(compressedContent, quality);
              quality -= 10;
              break;

            case ImageFormats.gif:
              quality = 0;
              break;

            default:
              compressedContent = ConvertPixelTypesToJpeg(compressedContent);
              extension = ImageFormats.jpeg;
              break;
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
