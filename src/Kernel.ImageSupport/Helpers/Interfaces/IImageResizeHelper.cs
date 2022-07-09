using LT.DigitalOffice.Kernel.Attributes;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.ImageSupport.Helpers.Interfaces
{
  [AutoInject]
  public interface IImageResizeHelper
  {
    Task<(bool isSuccess, string resizedContent, string extension)> ResizeAsync(string inputBase64, string extension, int resizeMinValue = 150);

    Task<(bool isSuccess, string resizedContent, string extension)> ResizePreciselyAsync(string inputBase64, string extension, int newWidth = 150, int newHeight = 150);
  }
}
