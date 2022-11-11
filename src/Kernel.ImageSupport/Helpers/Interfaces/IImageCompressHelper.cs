using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;

namespace DigitalOffice.Kernel.ImageSupport.Helpers.Interfaces;

/// <summary>
/// Represents interface for a helper.
/// Provides methods for compressing images.
/// </summary>
[AutoInject]
public interface IImageCompressHelper
{
  /// <summary>
  /// Compresses image to given maximal value.
  /// </summary>
  /// <remarks>
  /// Allows to compress images of following formats: ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tga", ".svg", ".webp", ".tiff", ".pbm".
  /// Compresses images step by step until receiving required weight value.
  /// Svg image on first iteration converts to png image.
  /// Png, bmp, gif, tga, webp, tiff, pbm images on first iteration converts to jpeg(jpg).
  /// Jpeg(jpg) on first iteration compresses with quality 90%,
  /// then on each iteration reduces quality by 10% (second iteration - 80%, third - 70% and so one);
  /// minimal quality value is 10% - compressing stops even if current size is bigger then required.
  /// If compressing is successful, returns boolean "true", compressed image content and final extension.
  /// If weight of original image is less than given maximal value, returns boolean "true", original image content and original extension.
  /// If compressing is not successful, returns boolean "false", null image content and original image extension.
  /// </remarks>
  /// <param name="inputBase64">
  /// Image original content.
  /// </param>
  /// <param name="extension">
  /// Image original extension.
  /// </param>
  /// <param name="maxSizeKb">
  /// Required maximal value of final weight in KB.
  /// </param>
  /// <returns>
  /// The value of <paramref name="isSuccess" />
  /// The value of <paramref name="compressedContent" />
  /// The value of <paramref name="extension" />
  /// </returns>
  /// <param name="isSuccess">A boolean.</param>
  /// <param name="compressedContent">Content of compressed image.</param>
  /// <param name="extension">Extension of compressed image.</param>
  Task<(bool isSuccess, string compressedContent, string extension)> CompressAsync(string inputBase64, string extension, int maxSizeKb);
}
