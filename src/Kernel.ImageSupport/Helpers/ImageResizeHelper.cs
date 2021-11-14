using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Svg;
using System;
using Drawing = System.Drawing;
using System.IO;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.ImageSupport.Constants;
using LT.DigitalOffice.Kernel.ImageSupport.Helpers.Interfaces;

namespace LT.DigitalOffice.Kernel.ImageSupport.Helpers
{
  public class ImageResizeHelper : IImageResizeHelper
  {
    #region private methods

    private Task<(bool isSuccess, string resizedContent, string extension)> SvgResize(string inputBase64, string extension, int resizeMinValue)
    {
      return Task.Run(() =>
      {
        try
        {
          var byteString = Convert.FromBase64String(inputBase64);

          using (var ms = new MemoryStream(byteString))
          {
            var svgDocument = SvgDocument.Open<SvgDocument>(ms);
            var image = svgDocument.Draw();

            if (image.Width <= resizeMinValue || image.Height <= resizeMinValue)
            {
              return (isSuccess: true, resizedContent: null, extension);
            }

            double maxSize = Math.Max(image.Width, image.Height);

            var ratio = Convert.ToInt32(Math.Ceiling(maxSize / resizeMinValue));

            var newImage = new Drawing.Bitmap(image.Width / ratio, image.Height / ratio);
            Drawing.Graphics.FromImage(newImage).DrawImage(image, 0, 0, image.Width / ratio, image.Height / ratio);

            var converter = new Drawing.ImageConverter();

            byteString = (byte[])converter.ConvertTo(newImage, typeof(byte[]));
            extension = ImageFormats.png;

            return (isSuccess: true,
              resizedContent: Convert.ToBase64String(byteString),
              extension);
          }
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

          var ratio = Convert.ToInt32(Math.Ceiling(maxSize / resizeMinValue));

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

    #endregion

    public async Task<(bool isSuccess, string resizedContent, string extension)> ResizeAsync(string inputBase64, string extension, int resizeMinValue = 150)
    {
      return string.Equals(extension, ImageFormats.svg, StringComparison.OrdinalIgnoreCase)
        ? await SvgResize(inputBase64, extension, resizeMinValue)
        : await BaseResize(inputBase64, extension, resizeMinValue);
    }
  }
}
