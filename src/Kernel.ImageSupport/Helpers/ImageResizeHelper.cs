using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.ImageSupport.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using Svg;
using Image = SixLabors.ImageSharp.Image;
using Rectangle = SixLabors.ImageSharp.Rectangle;

namespace LT.DigitalOffice.Kernel.ImageSupport.Helpers;

public class ImageResizeHelper : IImageResizeHelper
{
  private readonly ILogger<ImageResizeHelper> _logger;

  public const string Png = ".png";
  public const string Svg = ".svg";

  public ImageResizeHelper(ILogger<ImageResizeHelper> logger)
  {
    _logger = logger;
  }

  #region private methods

  private Task<(bool isSuccess, string resizedContent, string extension)> SvgResize(
    string inputBase64,
    string extension,
    int resizeMaxValue)
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
        extension = Png;

        return (isSuccess: true,
          resizedContent: Convert.ToBase64String(byteString),
          extension);
      }
      catch (Exception ex)
      {
        _logger.LogWarning("Can't resize svg-image: " + ex.Message);
        return (isSuccess: false, resizedContent: null, extension);
      }
    });
  }

  private Task<(bool isSuccess, string resizedContent, string extension)> SvgResizeForPreview(
    string inputBase64,
    string extension,
    int conditionalWidth,
    int conditionalHeight,
    int resizeMaxValue)
  {
    return Task.Run(() =>
    {
      try
      {
        byte[] byteString = Convert.FromBase64String(inputBase64);

        using MemoryStream ms = new MemoryStream(byteString);
        SvgDocument svgDocument = SvgDocument.Open<SvgDocument>(ms);
        Bitmap image = svgDocument.Draw();
        Bitmap croppedImage;

        if ((double)image.Width / image.Height > (double)conditionalWidth / conditionalHeight)
        {
          int targetWidth = image.Height * conditionalWidth / conditionalHeight;
          int targetHeight = image.Height;
          croppedImage = new(targetWidth, targetHeight);
          System.Drawing.Rectangle cropArea = new(image.Width / 2 - targetWidth / 2, 0, targetWidth, targetHeight);

          Graphics croppedImageGraphics = Graphics.FromImage(croppedImage);
          croppedImageGraphics.DrawImage(
            image,
            new System.Drawing.Rectangle(0, 0, targetWidth, targetHeight),
            cropArea,
            GraphicsUnit.Pixel);
        }
        else if ((double)image.Width / image.Height < (double)conditionalWidth / conditionalHeight)
        {
          int targetWidth = image.Width;
          int targetHeight = image.Width * conditionalHeight / conditionalWidth;
          croppedImage = new(targetWidth, targetHeight);
          System.Drawing.Rectangle cropArea = new(0, image.Height / 2 - targetHeight / 2, targetWidth, targetHeight);

          Graphics g = Graphics.FromImage(croppedImage);
          g.DrawImage(
            image,
            new System.Drawing.Rectangle(0, 0, targetWidth, targetHeight),
            cropArea,
            GraphicsUnit.Pixel);
        }
        else
        {
          croppedImage = image.Clone(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), image.PixelFormat);
        }

        double maxSize = Math.Max(croppedImage.Width, croppedImage.Height);

        if (maxSize <= resizeMaxValue)
        {
          return (isSuccess: true, resizedContent: null, extension);
        }

        double ratio = maxSize / resizeMaxValue;
        int newWidth = (int)(croppedImage.Width / ratio);
        int newHeight = (int)(croppedImage.Height / ratio);

        Bitmap resizedImage = new(newWidth, newHeight);
        System.Drawing.Rectangle newCropArea = new(0, 0, croppedImage.Width, croppedImage.Height);
        Graphics resizedImageGraphics = Graphics.FromImage(resizedImage);
        resizedImageGraphics.DrawImage(
          croppedImage,
          new System.Drawing.Rectangle(0, 0, newWidth, newHeight),
          newCropArea,
          GraphicsUnit.Pixel);

        ImageConverter converter = new ImageConverter();
        byteString = (byte[])converter.ConvertTo(resizedImage, typeof(byte[]));
        extension = Png;

        return (isSuccess: true,
          resizedContent: Convert.ToBase64String(byteString),
          extension);
      }
      catch (Exception ex)
      {
        _logger.LogWarning("Can't resize svg-image: " + ex.Message);
        return (isSuccess: false, resizedContent: null, extension);
      }
    });
  }

  private Task<(bool isSuccess, string resizedContent, string extension)> BaseResize(
    string inputBase64,
    string extension,
    int resizeMaxValue)
  {
    return Task.Run(() =>
    {
      try
      {
        Image image = Image.Load(Convert.FromBase64String(inputBase64), out IImageFormat imageFormat);

        double maxSize = Math.Max(image.Width, image.Height);

        if (maxSize <= resizeMaxValue)
        {
          return (isSuccess: true, resizedContent: null, extension);
        }

        double ratio = maxSize / resizeMaxValue;

        image.Mutate(x => x.Resize((int)(image.Width / ratio), (int)(image.Height / ratio)));

        return (isSuccess: true,
          resizedContent: image.ToBase64String(imageFormat).Split(',')[1],
          extension);
      }
      catch (Exception ex)
      {
        _logger.LogWarning("Can't resize image: content is damaged or format is wrong. " + ex.Message);
        return (isSuccess: false, resizedContent: null, extension);
      }
    });
  }

  private Task<(bool isSuccess, string resizedContent, string extension)> BaseResizeForPreview(
    string inputBase64,
    string extension,
    int conditionalWidth,
    int conditionalHeight,
    int resizeMaxValue)
  {
    return Task.Run(() =>
    {
      try
      {
        Image image = Image.Load(Convert.FromBase64String(inputBase64), out IImageFormat imageFormat);

        if ((double)image.Width / image.Height > (double)conditionalWidth / conditionalHeight)
        {
          int targetWidth = image.Height * conditionalWidth / conditionalHeight;
          int targetHeight = image.Height;
          Rectangle cropArea = new(image.Width / 2 - targetWidth / 2, 0, targetWidth, targetHeight);

          image.Mutate(x => x.Crop(cropArea));
        }
        else if ((double)image.Width / image.Height < (double)conditionalWidth / conditionalHeight)
        {
          int targetWidth = image.Width;
          int targetHeight = image.Width * conditionalHeight / conditionalWidth;
          Rectangle cropArea = new(0, image.Height / 2 - targetHeight / 2, targetWidth, targetHeight);

          image.Mutate(x => x.Crop(cropArea));
        }

        double maxSize = Math.Max(image.Width, image.Height);

        if (maxSize <= resizeMaxValue)
        {
          return (isSuccess: true, resizedContent: null, extension);
        }

        double ratio = maxSize / resizeMaxValue;

        image.Mutate(x => x.Resize((int)(image.Width / ratio), (int)(image.Height / ratio)));

        return (isSuccess: true,
          resizedContent: image.ToBase64String(imageFormat).Split(',')[1],
          extension);
      }
      catch (Exception ex)
      {
        _logger.LogWarning("Can't resize image: content is damaged or format is wrong. " + ex.Message);
        return (isSuccess: false, resizedContent: null, extension);
      }
    });
  }

  #endregion

  public Task<(bool isSuccess, string resizedContent, string extension)> ResizeAsync(
    string inputBase64,
    string extension,
    int resizeMaxValue = 150)
  {
    return string.Equals(extension, Svg, StringComparison.OrdinalIgnoreCase)
      ? SvgResize(inputBase64, extension, resizeMaxValue)
      : BaseResize(inputBase64, extension, resizeMaxValue);
  }

  public Task<(bool isSuccess, string resizedContent, string extension)> ResizeForPreviewAsync(
    string inputBase64,
    string extension,
    int conditionalWidth = 1,
    int conditionalHeight = 1,
    int resizeMaxValue = 150)
  {
    return string.Equals(extension, Svg, StringComparison.OrdinalIgnoreCase)
      ? SvgResizeForPreview(inputBase64, extension, conditionalWidth, conditionalHeight, resizeMaxValue)
      : BaseResizeForPreview(inputBase64, extension, conditionalWidth, conditionalWidth, resizeMaxValue);
  }
}
