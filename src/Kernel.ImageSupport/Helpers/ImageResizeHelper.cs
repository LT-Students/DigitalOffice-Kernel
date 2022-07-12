using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.ImageSupport.Constants;
using LT.DigitalOffice.Kernel.ImageSupport.Helpers.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Svg;
using Image = SixLabors.ImageSharp.Image;

namespace LT.DigitalOffice.Kernel.ImageSupport.Helpers
{
  public class ImageResizeHelper : IImageResizeHelper
  {
    #region private methods

    private Task<(bool isSuccess, string resizedContent, string extension)> SvgResize(string inputBase64, string extension, int resizeMaxValue)
    {
      return Task.Run(() =>
      {
        try
        {
          byte[] byteString = Convert.FromBase64String(inputBase64);

          using MemoryStream ms = new MemoryStream(byteString);
          SvgDocument svgDocument = SvgDocument.Open<SvgDocument>(ms);
          Bitmap image = svgDocument.Draw();

          double maxSize = Math.Max(image.Width, image.Height);

          if (maxSize <= resizeMaxValue)
          {
            return (isSuccess: true, resizedContent: null, extension);
          }

          double ratio = maxSize / resizeMaxValue;
          int newWidth = (int)(image.Width / ratio);
          int newHeight = (int)(image.Height / ratio);

          Bitmap newImage = new Bitmap(newWidth, newHeight);
          Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);

          ImageConverter converter = new ImageConverter();

          byteString = (byte[])converter.ConvertTo(newImage, typeof(byte[]));
          extension = ImageFormats.png;

          return (isSuccess: true,
            resizedContent: Convert.ToBase64String(byteString),
            extension);
        }
        catch
        {
          return (isSuccess: false, resizedContent: null, extension);
        }
      });
    }

    private Task<(bool isSuccess, string resizedContent, string extension)> SvgResizeForAvatar(string inputBase64, string extension, int conditionalWidth, int conditionalHeight, int resizeMaxValue)
    {
      return Task.Run(() =>
      {
        try
        {
          byte[] byteString = Convert.FromBase64String(inputBase64);

          using MemoryStream ms = new MemoryStream(byteString);
          SvgDocument svgDocument = SvgDocument.Open<SvgDocument>(ms);
          Bitmap image = svgDocument.Draw();
          Bitmap newImage = image;

          if ((double)image.Width / image.Height > (double)conditionalWidth / conditionalHeight)
          {
            int width = image.Height * conditionalWidth / conditionalHeight;
            newImage = new Bitmap(width, image.Height);
            Graphics g = Graphics.FromImage(newImage);
            g.DrawImage(image, -width, -image.Height);
          }
          else if ((double)image.Width / image.Height < (double)conditionalWidth / conditionalHeight)
          {
            int height = image.Height * conditionalWidth / conditionalHeight;
            newImage = new Bitmap(image.Width, height);
            Graphics g = Graphics.FromImage(newImage);
            g.DrawImage(image, -image.Width, -height);
          }

          double maxSize = Math.Max(newImage.Width, newImage.Height);

          if (maxSize <= resizeMaxValue)
          {
            return (isSuccess: true, resizedContent: null, extension);
          }

          double ratio = maxSize / resizeMaxValue;
          int newWidth = (int)(newImage.Width / ratio);
          int newHeight = (int)(newImage.Height / ratio);

          newImage = new Bitmap(newWidth, newHeight);
          Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);

          ImageConverter converter = new ImageConverter();

          byteString = (byte[])converter.ConvertTo(newImage, typeof(byte[]));
          extension = ImageFormats.png;

          return (isSuccess: true,
            resizedContent: Convert.ToBase64String(byteString),
            extension);
        }
        catch
        {
          return (isSuccess: false, resizedContent: null, extension);
        }
      });
    }

    private Task<(bool isSuccess, string resizedContent, string extension)> BaseResize(string inputBase64, string extension, int resizeMaxValue)
    {
      return Task.Run(() =>
      {
        try
        {
          Image image = Image.Load(Convert.FromBase64String(inputBase64));

          double maxSize = Math.Max(image.Width, image.Height);

          if (maxSize <= resizeMaxValue)
          {
            return (isSuccess: true, resizedContent: null, extension);
          }

          double ratio = maxSize / resizeMaxValue;

          image.Mutate(x => x.Resize((int)(image.Width / ratio), (int)(image.Height / ratio)));

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

    private Task<(bool isSuccess, string resizedContent, string extension)> BaseResizeForAvatar(string inputBase64, string extension, int conditionalWidth, int conditionalHeight, int resizeMaxValue)
    {
      return Task.Run(() =>
      {
        try
        {
          Image image = Image.Load(Convert.FromBase64String(inputBase64));

          if ((double)image.Width / image.Height > (double)conditionalWidth / conditionalHeight)
          {
            image.Mutate(x => x.Crop(image.Height * conditionalWidth / conditionalHeight, image.Height));
          }
          else if ((double)image.Width / image.Height < (double)conditionalWidth / conditionalHeight)
          {
            image.Mutate(x => x.Crop(image.Width, image.Width * conditionalHeight / conditionalWidth));
          }

          double maxSize = Math.Max(image.Width, image.Height);

          if (maxSize <= resizeMaxValue)
          {
            return (isSuccess: true, resizedContent: null, extension);
          }

          double ratio = maxSize / resizeMaxValue;

          image.Mutate(x => x.Resize((int)(image.Width / ratio), (int)(image.Height / ratio)));

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

    public Task<(bool isSuccess, string resizedContent, string extension)> ResizeAsync(string inputBase64, string extension, int resizeMaxValue = 150)
    {
      return string.Equals(extension, ImageFormats.svg, StringComparison.OrdinalIgnoreCase)
        ? SvgResize(inputBase64, extension, resizeMaxValue)
        : BaseResize(inputBase64, extension, resizeMaxValue);
    }

    public Task<(bool isSuccess, string resizedContent, string extension)> ResizeForAvatarAsync(string inputBase64, string extension, int conditionalWidth = 1, int conditionalHeight = 1, int resizeMaxValue = 150)
    {
      return string.Equals(extension, ImageFormats.svg, StringComparison.OrdinalIgnoreCase)
        ? SvgResizeForAvatar(inputBase64, extension, conditionalWidth, conditionalHeight, resizeMaxValue)
        : BaseResizeForAvatar(inputBase64, extension, conditionalWidth, conditionalWidth, resizeMaxValue);
    }
  }
}
