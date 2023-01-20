using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;

namespace DigitalOffice.Kernel.ImageSupport.Helpers.Interfaces;

/// <summary>
/// Represents interface for a helper.
/// Provides methods for resezing images.
/// </summary>
[AutoInject]
public interface IImageResizeHelper
{
  /// <summary>
  /// Reduces image to given maximal value with given height-width ratio.
  /// </summary>
  /// <remarks>
  /// Allows to resize images of following formats: ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tga", ".svg", ".webp", ".tiff", ".pbm".
  /// Has different private methods to resize vector-based format (".svg") and pixel-based formats (all other of allowed).
  /// If resizing of the svg image is successful, final image extension is ".png".
  /// If resizing of pixel-based image is successful, final image extension is equal to original one.
  /// If resizing is successful, returns boolean "true", resized image content and final extension.
  /// If width and height of original image is less than given maximal value, returns boolean "true", null image content and original image extension.
  /// If resizing is not successful, returns boolean "false", null image content and original image extension.
  /// </remarks>
  /// <param name="inputBase64">
  /// Image original content.
  /// </param>
  /// <param name="extension">
  /// Image original extension.
  /// </param>
  /// <param name="conditionalWidth">
  /// Required value of final image width, default value = 1.
  /// </param>
  /// <param name="conditionalHeight">
  /// Required value of final image height, default value = 1.
  /// </param>
  /// <param name="resizeMinValue">
  /// Required maximal value of final size, default value = 150.
  /// </param>
  /// <returns>
  /// The value of <paramref name="isSuccess" />
  /// The value of <paramref name="resizedContent" />
  /// The value of <paramref name="extension" />
  /// </returns>
  /// <param name="isSuccess">A boolean.</param>
  /// <param name="resizedContent">Content of resized image.</param>
  /// <param name="extension">Extension of resized image.</param>
  Task<(bool isSuccess, string resizedContent, string extension)> ResizeForPreviewAsync(
    string inputBase64, string extension, int conditionalWidth = 1, int conditionalHeight = 1, int resizeMaxValue = 150);

  /// <summary>
  /// Reduces image to given maximal value.
  /// </summary>
  /// <remarks>
  /// Allows to resize images of following formats: ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tga", ".svg", ".webp", ".tiff", ".pbm".
  /// Has different private methods to resize vector-based format (".svg") and pixel-based formats (all other of allowed).
  /// If resizing of the svg image is successful, final image extension is ".png".
  /// If resizing of pixel-based image is successful, final image extension is equal to original one.
  /// If resizing is successful, returns boolean "true", resized image content and final extension.
  /// If width and height of original image is less than given maximal value, returns boolean "true", null image content and original image extension.
  /// If resizing is not successful, returns boolean "false", null image content and original image extension.
  /// </remarks>
  /// <param name="inputBase64">
  /// Image original content.
  /// </param>
  /// <param name="extension">
  /// Image original extension.
  /// </param>
  /// <param name="resizeMinValue">
  /// Required maximal value of final size, default value = 150.
  /// </param>
  /// <returns>
  /// The value of <paramref name="isSuccess" />
  /// The value of <paramref name="resizedContent" />
  /// The value of <paramref name="extension" />
  /// </returns>
  /// <param name="isSuccess">A boolean.</param>
  /// <param name="resizedContent">Content of resized image.</param>
  /// <param name="extension">Extension of resized image.</param>
  Task<(bool isSuccess, string resizedContent, string extension)> ResizeAsync(string inputBase64, string extension, int resizeMaxValue = 150);
}
