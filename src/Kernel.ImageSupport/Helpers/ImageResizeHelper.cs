using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Svg;
using System;
using System.IO;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.ImageSupport.Constants;
using LT.DigitalOffice.Kernel.ImageSupport.Helpers.Interfaces;
using System.Drawing;
using Image = SixLabors.ImageSharp.Image;

namespace LT.DigitalOffice.Kernel.ImageSupport.Helpers
{
  public class ImageResizeHelper : IImageResizeHelper
  {
    #region private methods

    private (bool isSuccess, string resizedContent, string extension) MakeNewSvgImage(Bitmap image, int newWidth, int newHeight)
    {
      Bitmap newImage = new Bitmap(newWidth, newHeight);
      Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);

      ImageConverter converter = new ImageConverter();

      byte[] byteString = (byte[])converter.ConvertTo(newImage, typeof(byte[]));
      string extension = ImageFormats.png;

      return (isSuccess: true,
        resizedContent: Convert.ToBase64String(byteString),
        extension);
    }

    private Task<(bool isSuccess, string resizedContent, string extension)> SvgResize(string inputBase64, string extension, int resizeMinValue)
    {
      return Task.Run(() =>
      {
        try
        {
          byte[] byteString = Convert.FromBase64String(inputBase64);

          using MemoryStream ms = new MemoryStream(byteString);
          SvgDocument svgDocument = SvgDocument.Open<SvgDocument>(ms);
          Bitmap image = svgDocument.Draw();

          if (image.Width <= resizeMinValue || image.Height <= resizeMinValue)
          {
            return (isSuccess: true, resizedContent: null, extension);
          }

          double maxSize = Math.Max(image.Width, image.Height);

          int ratio = Convert.ToInt32(Math.Ceiling(maxSize / resizeMinValue));

          return MakeNewSvgImage(image, image.Width / ratio, image.Height / ratio);
        }
        catch
        {
          return (isSuccess: false, resizedContent: null, extension);
        }
      });
    }

    private Task<(bool isSuccess, string resizedContent, string extension)> SvgPreciselyResize(string inputBase64, string extension, int newWidth, int newHeight)
    {
      return Task.Run(() =>
      {
        try
        {
          byte[] byteString = Convert.FromBase64String(inputBase64);

          using MemoryStream ms = new MemoryStream(byteString);
          SvgDocument svgDocument = SvgDocument.Open<SvgDocument>(ms);
          Bitmap image = svgDocument.Draw();

          if (image.Width <= newWidth || image.Height <= newHeight)
          {
            return (isSuccess: true, resizedContent: null, extension);
          }

          return MakeNewSvgImage(image, newWidth, newHeight);
        }
        catch
        {
          return (isSuccess: false, resizedContent: null, extension);
        }
      });
    }

    private Task<(bool isSuccess, string resizedContent, string extension)> BaseResize(string inputBase64, string extension, int resizeMinValue)
    {
      return Task.Run(() =>
      {
        try
        {
          Image image = Image.Load(Convert.FromBase64String(inputBase64));

          if (image.Width <= resizeMinValue || image.Height <= resizeMinValue)
          {
            return (isSuccess: true, resizedContent: null, extension);
          }

          double maxSize = Math.Max(image.Width, image.Height);

          int ratio = Convert.ToInt32(Math.Ceiling(maxSize / resizeMinValue));

          image.Mutate(x => x.Resize(image.Width / ratio, image.Height / ratio));

          return (isSuccess: true,
            resizedContent: image.ToBase64String(FormatsDictionary.formatsInstances[extension]).Split(',')[1],
            extension);
        }
        catch
        {
          return (isSuccess: false, resizedContent: null, extension);
        }
      });
    }

    private Task<(bool isSuccess, string resizedContent, string extension)> BasePreciselyResize(string inputBase64, string extension, int newWidth, int newHeight)
    {
      return Task.Run(() =>
      {
        try
        {
          Image image = Image.Load(Convert.FromBase64String(inputBase64));

          if (image.Width <= newWidth || image.Height <= newHeight)
          {
            return (isSuccess: true, resizedContent: null, extension);
          }

          image.Mutate(x => x.Resize(newWidth, newHeight));

          return (isSuccess: true,
            resizedContent: image.ToBase64String(FormatsDictionary.formatsInstances[extension]).Split(',')[1],
            extension);
        }
        catch
        {
          return (isSuccess: false, resizedContent: null, extension);
        }
      });
    }

    #endregion

    public async Task<(bool isSuccess, string resizedContent, string extension)> ResizeAsync(string inputBase64, string extension, int resizeMinValue = 150)
    {
      return string.Equals(extension, ImageFormats.svg, StringComparison.OrdinalIgnoreCase)
        ? await SvgResize(inputBase64, extension, resizeMinValue)
        : await BaseResize(inputBase64, extension, resizeMinValue);
    }

    public async Task<(bool isSuccess, string resizedContent, string extension)> ResizePreciselyAsync(string inputBase64, string extension, int newWidth = 150, int newHeight = 150)
    {
      return string.Equals(extension, ImageFormats.svg, StringComparison.OrdinalIgnoreCase)
        ? await SvgPreciselyResize(inputBase64, extension, newWidth, newHeight)
        : await BasePreciselyResize(inputBase64, extension, newWidth, newHeight);
    }
  }
}
